using System.Collections.Generic;
using UnityEngine;

namespace ZeroStats.Game
{
    public class TextProvider : MonoBehaviour
    {
        [SerializeField] private List<string> textValues = new();
        private int _nextTextIndex = 0;

        public string GetNextText()
        {
            if (textValues.Count == 0) return "Нет текста";
            string next = textValues[_nextTextIndex];
            _nextTextIndex = (_nextTextIndex + 1) % textValues.Count;
            return next;
        }
    }
}