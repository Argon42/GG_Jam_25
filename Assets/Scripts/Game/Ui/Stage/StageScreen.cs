using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZeroStats.Game.Ui.Stage.Cards;

namespace ZeroStats.Game.Ui.Stage
{
    public class StageScreen : GameScreen
    {
        [SerializeField] private CardsHand cardsHand = default!;
        [SerializeField] private StageResult stageResult = default!;

        public void Show(List<Data.Remote.Card> cards, Action<Data.Remote.Card> onSelectCard)
        {
            gameObject.SetActive(true);
            stageResult.Hide();
            cardsHand.Show(cards, onSelectCard);
        }

        public UniTask ApplyEffect(Data.Remote.Card card)
        {
            cardsHand.RemoveCards();
            return stageResult.ShowResult(card);
        }

        public void Hide()
        {
            stageResult.Hide();
            gameObject.SetActive(false);
        }
    }
}