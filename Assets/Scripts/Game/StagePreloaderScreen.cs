using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ZeroStats.Game
{
    public class StagePreloaderScreen : GameScreen
    {
        [SerializeField] private TMP_Text textLabel = default!;
        [SerializeField] private new Animation animation = default!;

        [SerializeField] private int delayOfClose = 1000;
        [SerializeField] private int delayOfOpen = 1000;

        public async UniTask Show(StageState stage, Action onClose)
        {
            textLabel.text = GetTitle(stage);
            animation.Play();
            await UniTask.Delay(delayOfClose);
            onClose();
            await UniTask.Delay(delayOfOpen);
            animation.Rewind();
            animation.Stop();
        }

        private string GetTitle(StageState stage)
        {
            return stage.Current switch
            {
                GameStage.Morning => $"Утро {stage.Day + 1}",
                GameStage.Day => $"День {stage.Day + 1}",
                GameStage.Evening => $"Вечер {stage.Day + 1}",
                GameStage.Night => $"Ночь {stage.Day + 1}",
                _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null),
            };
        }
    }
}