using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using UnityEngine;

namespace ZeroStats.Game
{
    public class Player : MonoBehaviour
    {
        public const int MaxStatValueAbs = 20;
        private const int HandSizeDefault = 2;

        [SerializeField] private CardsConfig cardsConfig = default!;

        public readonly ReactiveProperty<GameStage> Stage = new(GameStage.None);
        public readonly ReactiveProperty<int> Day = new(0);

        public readonly ReactiveProperty<int> Stat1 = new(0);
        public readonly ReactiveProperty<int> Stat2 = new(0);
        public readonly ReactiveProperty<int> Stat3 = new(0);
        public readonly ReactiveProperty<int> Stat4 = new(0);

        private readonly Queue<StageState> _stages = new();
        private readonly HashSet<PlayerModificator> _modificators = new();

        public void ApplyEffect(Card card)
        {
            Stat1.Value += card.Stat1Delta;
            Stat2.Value += card.Stat2Delta;
            Stat3.Value += card.Stat3Delta;
            Stat4.Value += card.Stat4Delta;
        }

        public StageState GetNextStage()
        {
            return _stages.Count > 0 ? _stages.Dequeue() : CreateNewStage(Day.Value, Stage.Value);
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
            var result = new List<Card>();
            var cards = cardsConfig.Descriptors
                .Where(descriptor => !descriptor.NotApplicableStages.Contains(stageState.Current))
                .Where(descriptor => descriptor.StatNumber switch
                {
                    1 => descriptor.MinStatForUse <= Stat1.Value && descriptor.MaxStatForUse >= Stat1.Value,
                    2 => descriptor.MinStatForUse <= Stat2.Value && descriptor.MaxStatForUse >= Stat2.Value,
                    3 => descriptor.MinStatForUse <= Stat3.Value && descriptor.MaxStatForUse >= Stat3.Value,
                    4 => descriptor.MinStatForUse <= Stat4.Value && descriptor.MaxStatForUse >= Stat4.Value,
                    _ => false,
                })
                .ToList();

            for (int i = 0; i < neededCards; i++)
            {
                result.Add(GetCard(cards));
                cards.RemoveAll(descriptor => result.Any(card => card.Id == descriptor.Card.Id));
            }

            return result;
        }

        private Card GetCard(List<CardDescriptor> cards)
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
                    return cardDescriptor.Card;
            }

            return cards[0].Card;
        }

        public bool IsLoose() =>
            Mathf.Abs(Stat1.Value) >= MaxStatValueAbs ||
            Mathf.Abs(Stat2.Value) >= MaxStatValueAbs ||
            Mathf.Abs(Stat3.Value) >= MaxStatValueAbs ||
            Mathf.Abs(Stat4.Value) >= MaxStatValueAbs;
    }
}