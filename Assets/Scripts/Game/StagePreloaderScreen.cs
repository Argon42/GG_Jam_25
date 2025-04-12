using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace ZeroStats.Game
{
    public class StagePreloaderScreen : GameScreen
    {
        [SerializeField] private TMP_Text textLabel = default!;
        [SerializeField] private AnimationClip showAnimation = default!;
        [SerializeField] private AnimationClip hideAnimation = default!;
        [SerializeField] private new Animation animation = default!;

        [SerializeField] private int delayForReed = 1000;

        public async UniTask Show(StageState stage, Action onClose)
        {
            gameObject.SetActive(true);
            textLabel.text = G.LocalizeTitle(stage);
            animation.Rewind(showAnimation.name);
            animation.Play(showAnimation.name);
            await UniTask.Delay(showAnimation.Milliseconds());
            animation.Stop(showAnimation.name);
            onClose();
            await UniTask.Delay(delayForReed);
            animation.clip = hideAnimation;
            animation.Rewind();
            animation.Play();
            await UniTask.Delay(hideAnimation.Milliseconds());
            animation.Stop();
            gameObject.SetActive(false);
        }
    }
}