using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZeroStats.Common;
using ZeroStats.Game.Data;

namespace ZeroStats.Boot
{
    public class BootFlow : MonoBehaviour
    {
        private const string BaseUrl = "https://files.bananva.ru/data/gg_jam/";
        [SerializeField] private float delay = 0.3f;

        private void Start() => DelayedStart().Forget();

        private async UniTaskVoid DelayedStart()
        {
            try
            {
                var gameConfig = ScriptableObject.CreateInstance<GameConfig>();
                await gameConfig.GenerateParametersFromUrl(Path.Combine(BaseUrl, "config/{0}"));
                G.SetResourcesProvider(new OnlineGameResourceProvider(BaseUrl));
                G.ReplaceConfig(gameConfig);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                G.SetResourcesProvider(null);
                G.ReplaceConfig(null);
            }

            var delayUniTask = UniTask.Delay(TimeSpan.FromSeconds(delay));

            await delayUniTask;
            SceneManager.LoadSceneAsync(Scenes.Game);
        }
    }
}