using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Game
{
    [CreateAssetMenu(fileName = "CardsConfig", menuName = "ZeroStats/CardsConfig")]
    public class CardsConfig : ScriptableObject
    {
        [SerializeField] private GoogleSheetLoader.CardDatabase database = default!;

        public IReadOnlyList<CardDescriptor> Descriptors => database.cardDescriptors;

        public Card GetCard(int id) => database.cards.First(card => card.Id == id);

        [ContextMenu("Generate Descriptors")]
        public async UniTaskVoid GenerateDescriptors()
        {
            database = await new GoogleSheetLoader().Start();
        }
    }
}