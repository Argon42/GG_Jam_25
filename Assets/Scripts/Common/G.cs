using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
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

        public static void SetResourcesProvider(IGameResourceProvider? resourcesProvider) =>
            _resourcesProvider = resourcesProvider;

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

        public static void ReplaceConfig(GameConfig? gameConfig) => _config = gameConfig;
    }
}