using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Game
{
    public class StageScreen : GameScreen
    {
        public void Show(List<Card> cards, Action<Card> onSelectCard)
        {
            gameObject.SetActive(true);
        }

        public async UniTask ApplyEffect(Card card)
        {
        }
    }
}