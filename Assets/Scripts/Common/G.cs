using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using ZeroStats.Game;
using ZeroStats.Game.Data;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Common
{
    public static class G
    {
        private static GameConfig? _config;
        private static GameResourceProviderDefault _resourcesProviderDefault = new();
        private static IGameResourceProvider? _resourcesProvider;
        public static IGameConfig Config => _config ??= Resources.Load<GameConfig>("Config");
        private static IGameResourceProvider ResourcesProvider => _resourcesProvider ?? _resourcesProviderDefault;

        public static UniTask<Sprite> LoadSprite(string path) => ResourcesProvider.LoadSprite(path);
        public static async UniTaskVoid LoadSprite(string path, Action<Sprite> callback)
        {
            var sprite = await ResourcesProvider.LoadSprite(path);
            callback(sprite);
        }

        public static string Localize(string key) => key;

        public static string LocalizeTitle(StageState stage) => stage.Current switch
        {
            GameStage.Morning => $"Утро {stage.Day + 1}",
            GameStage.Day => $"День {stage.Day + 1}",
            GameStage.Evening => $"Вечер {stage.Day + 1}",
            GameStage.Night => $"Ночь {stage.Day + 1}",
            _ => throw new ArgumentOutOfRangeException(nameof(stage), stage, null),
        };

        public static int Milliseconds(this AnimationClip clip) => (int)(clip.length * 1000);

        public static Color LoadColor(string path) => Config.GetColor(path);

        public static bool KnowCard(int cardId) => PlayerPrefs.GetInt($"KnownCards/{cardId}", 0) != 0;

        public static void SetKnownCard(int cardId) => PlayerPrefs.SetInt($"KnownCards/{cardId}", 1);
    }

    internal class GameResourceProviderDefault : IGameResourceProvider
    {
        public UniTask<Sprite> LoadSprite(string path)
        {
            var sprite = Resources.Load<Sprite>(path);
            if (sprite == null)
            {
                Debug.LogError("sprite not found: " + path);
            }

            return UniTask.FromResult(sprite!);
        }
    }

    public interface IGameResourceProvider
    {
        UniTask<Sprite> LoadSprite(string path);
    }

    public class OnlineGameResourceProvider : IGameResourceProvider
    {
        private readonly string _baseUrl;
        private readonly Dictionary<string, Sprite> _spriteCache = new();

        public OnlineGameResourceProvider(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async UniTask<Sprite> LoadSprite(string path)
        {
            var url = $"{_baseUrl}/{path}";
            if (_spriteCache.TryGetValue(url, out var cachedSprite))
                return cachedSprite;

            using var www = UnityWebRequestTexture.GetTexture(url);
            var op = await www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load sprite from {url}: {www.error}");
                return null!;
            }

            var texture = DownloadHandlerTexture.GetContent(www);
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            _spriteCache[url] = sprite;
            return sprite;
        }
    }
}