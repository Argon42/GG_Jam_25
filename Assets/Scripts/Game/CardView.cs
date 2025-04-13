using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZeroStats.Common;

namespace ZeroStats.Game
{
    public class CardView : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image image = default!;
        [SerializeField] private TMP_Text text = default!;
        [SerializeField] private Button button = default!;
        [SerializeField] private DeltaEffectView deltaEffectViewPrefab = default!;
        [SerializeField] private RectTransform deltaEffectContainer = default!;
        [SerializeField] private float angle = 10;
        [SerializeField] private float speed = 5f;

        private Coroutine? _enterCoroutine;

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            _enterCoroutine = StartCoroutine(PointerEnter());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopCoroutine(_enterCoroutine);
            transform.rotation = Quaternion.identity;
        }

        private IEnumerator PointerEnter()
        {
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                float time = 0f;
                while (true)
                {
                    time += Time.deltaTime * speed;
                    float z = Mathf.Sin(time) * angle;
                    transform.rotation = Quaternion.Euler(0, 0, z);
                    yield return null;
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            throw new NotImplementedException();
        }
    }
}