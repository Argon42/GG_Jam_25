using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZeroStats.Game.Data.Remote;

namespace ZeroStats.Game.Data
{
    public interface IGameConfig
    {
        IReadOnlyList<CardDescriptor> Descriptors { get; }
        IReadOnlyList<EngGameResults> EngResults { get; }
        Card GetCard(int id);
        int GetInt(string paramName);
        float GetFloat(string paramName);
        Color GetColor(string colorName);

        /// <summary>
        /// Обновить базу данных с помощью форматированной ссылки с 1 аргументом для таблицы
        /// </summary>
        UniTask GenerateParametersFromUrl(string urlFormat);
    }
}