using System;
using UnityEngine;
using ZeroStats.Game;

namespace ZeroStats.Common
{
    public static class G
    {
        private static GameConfig? _config;
        public static IGameConfig Config => _config ??= Resources.Load<GameConfig>("Config");

        public static Sprite LoadSprite(string path) => Resources.Load<Sprite>(path);
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
    }
}