using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Common
{
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
}