using System.Collections.Generic;
using UnityEngine;

namespace ZeroStats.Game
{
    [CreateAssetMenu(fileName = "CardsConfig", menuName = "ZeroStats/CardsConfig")]
    public class CardsConfig : ScriptableObject
    {
        public List<CardDescriptor> Descriptors { get; } = new();
    }
}