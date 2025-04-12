using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Game
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ZeroStats/CardsConfig")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [SerializeField] private GoogleSheetLoader.CardDatabase database = default!;

        public IReadOnlyList<CardDescriptor> Descriptors => database.cardDescriptors;

        public Card GetCard(int id) => database.cards.First(card => card.Id == id);
        public int GetInt(string paramName) => database.parameters.GetInt(paramName);
        public float GetFloat(string paramName) => database.parameters.GetFloat(paramName);
        public Color GetColor(string colorName) => database.colors.First(color => color.name == colorName).color;

        [ContextMenu("Generate Descriptors")]
        public async UniTaskVoid GenerateDescriptors()
        {
            database = await new GoogleSheetLoader().LoadAllData();
        }
    }
}