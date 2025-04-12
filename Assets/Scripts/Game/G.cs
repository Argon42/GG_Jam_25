using System;
using UnityEngine;

namespace ZeroStats.Game
{
    public static class G
    {
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
    }
}