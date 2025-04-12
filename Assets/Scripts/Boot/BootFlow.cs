using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeroStats.Common;

namespace ZeroStats.Boot
{
    public class BootFlow : MonoBehaviour
    {
        [SerializeField] private float delay = 0.3f;

        private void Start() => StartCoroutine(DelayedStart());

        private IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadSceneAsync(Scenes.Game);
        }
    }
}