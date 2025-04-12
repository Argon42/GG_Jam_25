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
            await UniTask.Delay(showAnimation.Milliseconds());

            await UniTask.WhenAny(UniTask.Delay(delay), UniTask.WaitUntil(() => Input.GetMouseButtonDown(0)));
            animation.clip = hideAnimation;
            animation.Play();
            await UniTask.Delay(hideAnimation.Milliseconds());
            gameObject.SetActive(false);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}