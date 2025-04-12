using System.Collections.Generic;

namespace ZeroStats.Game
{
    public interface IGameConfig
    {
        IReadOnlyList<CardDescriptor> Descriptors { get; }
        Card GetCard(int id);
    }
}