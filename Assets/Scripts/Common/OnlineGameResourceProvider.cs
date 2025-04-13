using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace ZeroStats.Common
{
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
            var url = Path.Combine(_baseUrl, path + ".png");
            if (_spriteCache.TryGetValue(url, out var cachedSprite))
                return cachedSprite;

            try
            {
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
            catch
            {
                return null!;
            }
        }
    }
}