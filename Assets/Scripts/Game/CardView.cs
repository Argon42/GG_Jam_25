using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ZeroStats.Game
{
    public class CardView : MonoBehaviour
    {
        [SerializeField] private Image image = default!;
        [SerializeField] private TextMeshProUGUI text = default!;
        [SerializeField] private Button button = default!;


        public void Show(Card card, Func<Vector2> position, Action onSelect)
        {
            transform.localPosition = position.Invoke();
            image.sprite = G.LoadSprite(card.IconPath);
            text.text = G.Localize(card.Name);
            button.onClick.AddListener(onSelect.Invoke);
        }

        public void Hide(Action onEnd)
        {
            button.onClick.RemoveAllListeners();
            onEnd.Invoke();
        }
    }
}