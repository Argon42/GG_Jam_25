using System;
using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.UI;
using ZeroStats.Common;
using ZeroStats.Game.Data.Enums;
using Random = UnityEngine.Random;

namespace ZeroStats.Game.Ui.Stage.Stats
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private Image leftBar = default!;
        [SerializeField] private Image rightBar = default!;
        [SerializeField] private Image icon = default!;
        private IDisposable? _subscription;

        public void SetStat(ReadOnlyReactiveProperty<int> stat, int maxValue, StatType type)
        {
            _subscription?.Dispose();

            G.LoadSprite($"Icons/{type.ToString()}", sprite => icon.sprite = sprite).Forget();
            leftBar.color = G.LoadColor($"ColorsBars/{type.ToString()}");
            rightBar.color = G.LoadColor($"ColorsBars/{type.ToString()}");

            _subscription = stat.Subscribe(v =>
            {
                leftBar.DOFillAmount(CalculateNormalizedForLeft(maxValue, v), 0.5f + Random.Range(-0.3f, 0.3f));
                rightBar.DOFillAmount(CalculateNormalizedForRight(maxValue, v), 0.5f + Random.Range(-0.3f, 0.3f));
            }).AddTo(this);
        }

        private static float CalculateNormalizedForRight(int maxValue, int v)
        {
            return Mathf.Max(v, 0) / (float)maxValue;
        }

        private static float CalculateNormalizedForLeft(int maxValue, int v)
        {
            return Mathf.Abs(Mathf.Min(v, 0)) / (float)maxValue;
        }
    }
}