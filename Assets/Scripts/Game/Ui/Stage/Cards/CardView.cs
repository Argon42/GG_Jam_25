using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZeroStats.Common;
using ZeroStats.Game.Data.Enums;
using ZeroStats.Game.Data.Remote;
using Random = UnityEngine.Random;

namespace ZeroStats.Game.Ui.Stage.Cards
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
        [SerializeField] private AnimationSoundEventHandler audioSource = default!;
        [SerializeField] private AspectRatioFitter fitter = default!;

        private Coroutine? _enterCoroutine;

        public void Show(Card card, Func<Vector2> position, Action onSelect)
        {
            transform.localPosition = position.Invoke();
            G.LoadSprite(card.IconPath, sprite =>
            {
                image.sprite = sprite;
                fitter.aspectRatio = (float) sprite.texture.width / sprite.texture.height;
            }).Forget();
            text.text = G.Localize(card.Name);
            button.onClick.AddListener(onSelect.Invoke);
            var color = glow.color;
            color.a = 0f;
            glow.color = color;

            CreateDeltaEffectView(card);
        }

        private void CreateDeltaEffectView(Card card)
        {
            if (card.Stat1Delta != 0)
                Instantiate(deltaEffectViewPrefab, deltaEffectContainer)
                    .Show(card.Stat1Delta, StatType.First, G.KnowCard(card.Id));

            if (card.Stat2Delta != 0)
                Instantiate(deltaEffectViewPrefab, deltaEffectContainer)
                    .Show(card.Stat2Delta, StatType.Second, G.KnowCard(card.Id));

            if (card.Stat3Delta != 0)
                Instantiate(deltaEffectViewPrefab, deltaEffectContainer)
                    .Show(card.Stat3Delta, StatType.Third, G.KnowCard(card.Id));

            if (card.Stat4Delta != 0)
                Instantiate(deltaEffectViewPrefab, deltaEffectContainer)
                    .Show(card.Stat4Delta, StatType.Fourth, G.KnowCard(card.Id));
        }

        public async void Hide(Action onEnd)
        {
            button.onClick.RemoveAllListeners();
            var outTime = 0.6f + Random.Range(-0.2f, 0.2f);
            DOTween.Sequence()
                .Insert(0, canvasGroup.DOFade(0f, outTime))
                .Insert(0, transform.DOScale(Vector3.zero, outTime))
                .Insert(0, transform.DORotate(Vector3.forward * 50 * (Random.value > 0.5f ? 1f : -1f), outTime))
                .OnComplete(onEnd.Invoke);
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
        [SerializeField] private CanvasGroup canvasGroup;

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
            audioSource.PlaySound();
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