using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZeroStats.Common;
using ZeroStats.Game.Data;
using ZeroStats.Game.Data.Enums;
using ZeroStats.Game.Data.Remote;
using Random = UnityEngine.Random;

namespace ZeroStats.Game.Ui
{
    public class ResultScreen : GameScreen
    {
        [SerializeField] private Button restartButton = default!;
        [SerializeField] private Image resultImage = default!;
        [SerializeField] private AspectRatioFitter aspectRatioFitter = default!;
        [SerializeField] private TMP_Text resultText = default!;

        private Action? _onRestart;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestart);
        }

        public void Show(Action onRestart, HashSet<(StatType stat, bool isPositive)> results, StageState stage)
        {
            gameObject.SetActive(true);
            _onRestart = onRestart;

            var gameResults = G.Config.EngResults
                .OrderBy(_ => Random.value
                )
                .FirstOrDefault(gameResults => gameResults.requiredStats
                    .Zip(gameResults.requiredValues, (type, i) =>
                        results.Any(result =>
                            result.stat == type
                            && ((result.isPositive ? i > 0 : i < 0) || i == 0)))
                    .All(b => b) && gameResults.minDay <= stage.Day) ?? new EngGameResults
            {
                id = default,
                text = "Ты умер при странных обстоятельствах",
                iconPath = "placeholder",
                requiredStats = Array.Empty<StatType>(),
                requiredValues = Array.Empty<int>(),
                minDay = default,
            };

            resultText.text = gameResults.text;
            G.LoadSprite(gameResults.iconPath, sprite =>
            {
                resultImage.sprite = sprite;
                aspectRatioFitter.aspectRatio = sprite.rect.width / sprite.rect.height;
            }).Forget();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnRestart()
        {
            _onRestart?.Invoke();
        }
    }
}