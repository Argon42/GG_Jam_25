using System;
using System.Collections.Generic;
using UnityEngine;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game.Ui.Stage.Cards
{
    public class CardsHand : MonoBehaviour
    {
        [SerializeField] private CardView cardPrefab = default!;
        [SerializeField] private RectTransform cardsContainer = default!;

        private readonly List<CardView> _cardViews = new();
        private Action<Card>? _onSelectedCard;

        public void Show(List<Card> cards, Action<Card> onSelectCard)
        {
            DestroyCards(true);
            _onSelectedCard = onSelectCard;

            foreach (var card in cards)
            {
                var cardView = Instantiate(cardPrefab, cardsContainer);
                cardView.Show(card, () => GetPositionForCard(cards.IndexOf(card), cards.Count),
                    () => _onSelectedCard?.Invoke(card));
                _cardViews.Add(cardView);
            }
        }

        private Vector2 GetPositionForCard(int indexOf, int cardsCount)
        {
            var rect = cardsContainer.rect;
            var cardWidth = rect.width / cardsCount;
            var x = cardWidth * indexOf - cardWidth / 2f;
            var y = rect.height / 2f - rect.height / 2f;
            return new Vector2(x, y);
        }

        public void RemoveCards()
        {
            DestroyCards(false);
        }

        private void DestroyCards(bool immediate)
        {
            foreach (var cardView in _cardViews)
            {
                if (immediate)
                {
                    Destroy(cardView.gameObject);
                }
                else
                {
                    cardView.Hide(() => Destroy(cardView.gameObject));
                }
            }

            _cardViews.Clear();
        }
    }
}