using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ZeroStats.Game
{
    public class StagePreloaderScreen : GameScreen
    {
        [SerializeField] private TMP_Text textLabel = default!;
        [SerializeField] private AnimationClip showAnimation = default!;
        [SerializeField] private AnimationClip hideAnimation = default!;
        [SerializeField] private new Animation animation = default!;

        [SerializeField] private int delayOfClose = 1000;
        [SerializeField] private int delayOfOpen = 1000;

        public async UniTask Show(StageState stage, Action onClose)
        {
            gameObject.SetActive(true);
            textLabel.text = G.LocalizeTitle(stage);
            animation.clip = showAnimation;
            animation.Play();
            await UniTask.Delay(showAnimation.Milliseconds());
            onClose();
            await UniTask.Delay(hideAnimation.Milliseconds());
            animation.Rewind();
            animation.Stop();
            gameObject.SetActive(false);
        }
    }
}