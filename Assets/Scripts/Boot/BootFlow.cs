using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeroStats.Common;

namespace ZeroStats.Boot
{
    public class BootFlow : MonoBehaviour
    {
        [SerializeField] private int delay = 1;

        private IEnumerable Start()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadSceneAsync(Scenes.Game);
        }
    }
}