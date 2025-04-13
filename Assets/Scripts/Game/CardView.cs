using System;
using System.Collections;
using DG.Tweening;
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
        [SerializeField] private Image glow = default!;

        private Coroutine? _enterCoroutine;

        public void Show(Card card, Func<Vector2> position, Action onSelect)
        {
            transform.localPosition = position.Invoke();
            image.sprite = G.LoadSprite(card.IconPath);
            text.text = G.Localize(card.Name);
            button.onClick.AddListener(onSelect.Invoke);
            var color = glow.color;
            color.a = 0f;
            glow.color = color;
        }

        public void Hide(Action onEnd)
        {
            button.onClick.RemoveAllListeners();
            onEnd.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _enterCoroutine = StartCoroutine(PointerEnter());
            isPointerOver = true;
            if (isHeld)
                StartGlow();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopCoroutine(_enterCoroutine);
            transform.rotation = Quaternion.identity;

            isPointerOver = false;
            StopGlow();
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


        [SerializeField] private float maxAlpha = 1f;
        [SerializeField] private float pulseAmount = 0.2f;
        [SerializeField] private float fadeInTime = 0.2f;
        [SerializeField] private float fadeOutTime = 0.1f;
        [SerializeField] private float pulseSpeed = 1f;

        private Tween? glowTween;
        private bool isHeld = false;
        private bool isPointerOver = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            isHeld = true;
            isPointerOver = true;
            StartGlow();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isHeld = false;
            StopGlow();
        }

        private void StartGlow()
        {
            glowTween?.Kill();

            Color c = glow.color;
            c.a = 0f;
            glow.color = c;

            glowTween = DOTween.Sequence()
                .Append(glow.DOFade(maxAlpha, fadeInTime))
                .Append(glow.DOFade(maxAlpha - pulseAmount, 1f / pulseSpeed)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine));
        }

        private void StopGlow()
        {
            glowTween?.Kill();
            glowTween = glow.DOFade(0f, fadeOutTime);
        }
    }
}