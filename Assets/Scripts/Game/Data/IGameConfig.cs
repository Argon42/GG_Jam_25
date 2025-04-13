using System.Collections.Generic;
using UnityEngine;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game.Data
{
    public interface IGameConfig
    {
        IReadOnlyList<CardDescriptor> Descriptors { get; }
        Card GetCard(int id);
        int GetInt(string paramName);
        float GetFloat(string paramName);
        Color GetColor(string colorName);
    }
}