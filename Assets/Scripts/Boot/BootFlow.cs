using System;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
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

            await UniTask.Delay(TimeSpan.FromSeconds(delay));

            LoadSceneFromBundleAsync(Path.Combine(BaseUrl, "game.unity3d"), Scenes.Game).Forget();
        }

        // bundleURL – URL AssetBundle, fallbackSceneName – имя запасной сцены из сборки проекта
        public async UniTask LoadSceneFromBundleAsync(string bundleURL, string fallbackSceneName)
        {
            try
            {
                using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(bundleURL))
                {
                    await request.SendWebRequest().ToUniTask();

                    if (request.result != UnityWebRequest.Result.Success)
                    {
                        Debug.LogError($"Ошибка загрузки бандла: {request.error}");
                        SceneManager.LoadScene(fallbackSceneName);
                        return;
                    }

                    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
                    if (bundle == null)
                    {
                        Debug.LogError("Не удалось получить AssetBundle.");
                        SceneManager.LoadScene(fallbackSceneName);
                        return;
                    }

                    string[] scenePaths = bundle.GetAllScenePaths();
                    if (scenePaths.Length == 0)
                    {
                        Debug.LogError("Сцены в бандле не найдены.");
                        SceneManager.LoadScene(fallbackSceneName);
                        bundle.Unload(false);
                        return;
                    }

                    string scenePath = scenePaths[0]; // Выбор первой сцены из бандла
                    AsyncOperation loadOperation = SceneManager.LoadSceneAsync(scenePath);
                    await loadOperation.ToUniTask();

                    if (!loadOperation.isDone)
                    {
                        Debug.LogError("Сцена не загрузилась полностью.");
                        SceneManager.LoadScene(fallbackSceneName);
                    }

                    bundle.Unload(false);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Исключение при загрузке сцены: {ex.Message}");
                SceneManager.LoadScene(fallbackSceneName);
            }
        }
    }
}