﻿using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;
using ZeroStats.Common;
using ZeroStats.Game.Data;
using ZeroStats.Game.Data.Enums;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game
{
    public class Player : MonoBehaviour
    {
        public static int MaxStatValueAbs => G.Config.GetInt("MaxStatValue");
        private static int HandSizeDefault => G.Config.GetInt("HandSizeDefault");

        public readonly ReactiveProperty<GameStage> Stage = new(GameStage.None);
        public readonly ReactiveProperty<int> Day = new(0);

        public readonly ReactiveProperty<int> Stat1 = new(0);
        public readonly ReactiveProperty<int> Stat2 = new(0);
        public readonly ReactiveProperty<int> Stat3 = new(0);
        public readonly ReactiveProperty<int> Stat4 = new(0);
        public ReactiveProperty<StageState> CurrentStage { get; } = new();

        private readonly List<StageState> _stages = new();
        private readonly HashSet<PlayerModificator> _modificators = new();


        public void ApplyEffect(Card card)
        {
            Stat1.Value += card.Stat1Delta;
            Stat2.Value += card.Stat2Delta;
            Stat3.Value += card.Stat3Delta;
            Stat4.Value += card.Stat4Delta;

            if (card.AddCardIdInPool != 0)
            {
                GetStage(card.StageDelayForAddToPool)
                    .AdditionalCards
                    .Add(G.Config.GetCard(card.AddCardIdInPool));
            }

            G.SetKnownCard(card.Id);
        }

        private StageState GetStage(int offset)
        {
            var stageState = CurrentStage.Value;
            while (_stages.Count < offset + 1)
            {
                stageState = CreateNewStage(stageState.Day, stageState.Current);
                _stages.Add(stageState);
            }

            return _stages[offset];
        }

        public StageState ChangeStage()
        {
            var stageState = _stages.Count > 0 ? Dequeue(_stages) : CreateNewStage(Day.Value, Stage.Value);
            Day.Value = stageState.Day;
            Stage.Value = stageState.Current;
            CurrentStage.Value = stageState;
            return stageState;
        }

        private static StageState CreateNewStage(int dayValue, GameStage stageValue)
        {
            var nextStage = stageValue switch
            {
                GameStage.None => GameStage.Morning,
                GameStage.StartGame => GameStage.Morning,
                GameStage.Morning => GameStage.Day,
                GameStage.Day => GameStage.Evening,
                GameStage.Evening => GameStage.Night,
                GameStage.Night => GameStage.Morning,
                GameStage.Result => GameStage.Morning,
                _ => throw new ArgumentOutOfRangeException(nameof(stageValue), stageValue, null),
            };

            if (stageValue == GameStage.Night)
            {
                dayValue += 1;
            }

            return new StageState(nextStage, dayValue, HandSizeDefault,
                CalculateStageModificators(nextStage, dayValue));
        }

        private static HashSet<StageModificator> CalculateStageModificators(GameStage nextStage, int dayValue)
        {
            return new HashSet<StageModificator>();
        }

        public void StartNewGame()
        {
            Stat1.Value = 0;
            Stat2.Value = 0;
            Stat3.Value = 0;
            Stat4.Value = 0;
            Day.Value = 0;
            Stage.Value = GameStage.None;
            _stages.Clear();
            _modificators.Clear();
        }

        public List<Card> GetCards(StageState stageState)
        {
            var neededCards = Mathf.Max(stageState.HandSize - stageState.AdditionalCards.Count, 0);
            return stageState
                .AdditionalCards
                .Take(stageState.HandSize)
                .Concat(GenerateCardsForStage(stageState, neededCards))
                .ToList();
        }

        private IEnumerable<Card> GenerateCardsForStage(StageState stageState, int neededCards)
        {
            IGameConfig gameConfig = G.Config;
            var result = new List<Card>();
            var cards = gameConfig.Descriptors
                .Where(descriptor => !descriptor.NotApplicableStages.Contains(stageState.Current))
                .Where(descriptor => descriptor.StatNumber switch
                {
                    1 => descriptor.MinStatForUse <= Stat1.Value && descriptor.MaxStatForUse >= Stat1.Value,
                    2 => descriptor.MinStatForUse <= Stat2.Value && descriptor.MaxStatForUse >= Stat2.Value,
                    3 => descriptor.MinStatForUse <= Stat3.Value && descriptor.MaxStatForUse >= Stat3.Value,
                    4 => descriptor.MinStatForUse <= Stat4.Value && descriptor.MaxStatForUse >= Stat4.Value,
                    _ => true,
                })
                .ToList();

            for (int i = 0; i < neededCards; i++)
            {
                var card = gameConfig.GetCard(GetCard(cards));
                result.Add(card);
                cards.RemoveAll(descriptor => result.Any(cardInResults =>
                {
                    var cardFromDes = gameConfig.GetCard(descriptor.CardId);
                    return cardInResults.Id == descriptor.CardId
                           || (cardInResults.Group == cardFromDes.Group && cardFromDes.Group != 0);
                }));
            }

            return result;
        }

        private int GetCard(List<CardDescriptor> cards)
        {
            if (cards.Count == 0)
                throw new InvalidOperationException("No cards left to select from.");

            float totalWeight = cards.Sum(card => card.Weight);
            float randomValue = UnityEngine.Random.Range(0f, totalWeight);

            float cumulative = 0f;
            foreach (var cardDescriptor in cards)
            {
                cumulative += cardDescriptor.Weight;
                if (randomValue < cumulative)
                    return cardDescriptor.CardId;
            }

            return cards[0].CardId;
        }

        public bool IsLoose(out HashSet<(StatType stat, bool isPositive)> unknown)
        {
            var isFirstStat = Mathf.Abs(Stat1.Value) >= MaxStatValueAbs;
            var isSecondStat = Mathf.Abs(Stat2.Value) >= MaxStatValueAbs;
            var isThirdStat = Mathf.Abs(Stat3.Value) >= MaxStatValueAbs;
            var isFourthStat = Mathf.Abs(Stat4.Value) >= MaxStatValueAbs;

            unknown = new HashSet<(StatType stat, bool isPositive)>();
            if (isFirstStat) unknown.Add((StatType.First, Stat1.Value > 0));
            if (isSecondStat) unknown.Add((StatType.Second, Stat2.Value > 0));
            if (isThirdStat) unknown.Add((StatType.Third, Stat3.Value > 0));
            if (isFourthStat) unknown.Add((StatType.Fourth, Stat4.Value > 0));

            return isFirstStat || isSecondStat || isThirdStat || isFourthStat;
        }

        private static StageState Dequeue(List<StageState> stages)
        {
            var stageState = stages.First();
            stages.RemoveAt(0);
            return stageState;
        }
    }
}