using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Common
{
    public interface IGameResourceProvider
    {
        UniTask<Sprite> LoadSprite(string path);
    }
}