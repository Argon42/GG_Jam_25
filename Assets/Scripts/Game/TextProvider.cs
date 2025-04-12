using System.Collections.Generic;
using UnityEngine;

namespace ZeroStats.Game
{
    public class TextProvider : MonoBehaviour
    {
        [SerializeField] private List<string> textValues = new();

        public string GetNextText()
        {
            if (textValues.Count == 0) return "Нет текста";
            return $"{textValues[Random.Range(0, textValues.Count)]} - 0";
        }
    }
}