using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using ZeroStats.Common;

namespace ZeroStats.Game
{
    public class StageResult : MonoBehaviour
    {
        [SerializeField] private AnimationClip showAnimation = default!;
        [SerializeField] private AnimationClip hideAnimation = default!;
        [SerializeField] private new Animation animation = default!;
        [SerializeField] private TMP_Text resultText = default!;
        [SerializeField] private int delay = 1000;

        public async UniTask ShowResult(Card card)
        {
            gameObject.SetActive(true);
            resultText.text = G.Localize(card.ResultDescription);
            animation.clip = showAnimation;
            animation.Play();
            await UniTask.Delay(delay + showAnimation.Milliseconds());
            animation.clip = hideAnimation;
            animation.Play();
            await UniTask.Delay(delay + hideAnimation.Milliseconds());
            gameObject.SetActive(false);
        }
    }
}