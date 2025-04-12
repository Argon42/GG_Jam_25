using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Game
{
    public class StageScreen : GameScreen
    {
        [SerializeField] private CardsHand cardsHand = default!;
        [SerializeField] private StageResult stageResult = default!;

        public void Show(List<Card> cards, Action<Card> onSelectCard)
        {
            gameObject.SetActive(true);
            stageResult.Hide();
            cardsHand.Show(cards, onSelectCard);
        }

        public UniTask ApplyEffect(Card card)
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