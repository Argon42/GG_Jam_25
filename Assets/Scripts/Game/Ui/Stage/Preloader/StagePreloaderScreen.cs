using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using ZeroStats.Common;
using ZeroStats.Game.Data;
using Random = UnityEngine.Random;

namespace ZeroStats.Game.Ui.Stage.Preloader
{
    public class StagePreloaderScreen : GameScreen
    {
        [SerializeField] private TMP_Text textLabel = default!;
        [SerializeField] private AnimationClip showAnimation = default!;
        [SerializeField] private AnimationClip hideAnimation = default!;
        [SerializeField] private new Animation animation = default!;
        [SerializeField] private AudioSource audioSource = default!;

        [SerializeField] private int delayForReed = 1000;
        [SerializeField] private AudioClip[] sound = default!;

        public async UniTask Show(StageState stage, Action onClose)
        {
            gameObject.SetActive(true);
            textLabel.text = G.LocalizeTitle(stage);
            PlayAnimation(showAnimation.name);
            PlaySound(sound[Random.Range(0, sound.Length)]);
            await UniTask.Delay(showAnimation.Milliseconds());
            animation.Stop(showAnimation.name);
            onClose();
            await UniTask.Delay(delayForReed);
            PlaySound(sound[Random.Range(0, sound.Length)]);
            PlayAnimation(hideAnimation.name);
            await UniTask.Delay(hideAnimation.Milliseconds());
            animation.Stop();
            gameObject.SetActive(false);
        }

        private void PlayAnimation(string animationName)
        {
            animation.Stop();
            animation.Rewind();
            animation.Rewind(animationName);
            animation.Play(animationName);
        }

        private void PlaySound(AudioClip clip)
        {
            audioSource.clip = clip;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
    }
}