using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game.Data
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "ZeroStats/CardsConfig")]
    public class GameConfig : ScriptableObject, IGameConfig
    {
        [SerializeField] private CardDatabase database = default!;

        public IReadOnlyList<CardDescriptor> Descriptors => database.cardDescriptors;
        public IReadOnlyList<EngGameResults> EngResults => database.engResults;

        public Card GetCard(int id) => database.cards.First(card => card.Id == id);
        public int GetInt(string paramName) => database.parameters.GetInt(paramName);
        public float GetFloat(string paramName) => database.parameters.GetFloat(paramName);
        public Color GetColor(string colorName) => database.colors.First(color => color.name == colorName).color;

        [ContextMenu("Update database from Google")]
        private async UniTaskVoid GenerateDescriptorsFromGoogle() =>
            database = await new GoogleSheetLoader().LoadAllData();

        /// <summary>
        /// Обновить базу данных с помощью форматированной ссылки с 1 аргументом для таблицы
        /// </summary>
        public async UniTask GenerateParametersFromUrl(string urlFormat) =>
            database = await new GoogleSheetLoader(arg => string.Format(urlFormat, arg)).LoadAllData();
    }
}