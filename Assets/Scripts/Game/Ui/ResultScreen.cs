using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Game.Ui
{
    public class ResultScreen : GameScreen
    {
        [SerializeField] private Button restartButton = default!;
        private Action? _onRestart;

        private void Awake()
        {
            restartButton.onClick.AddListener(OnRestart);
        }

        public void Show(Action onRestart, HashSet<(StatType stat, bool isPositive)> results)
        {
            gameObject.SetActive(true);
            _onRestart = onRestart;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnRestart()
        {
            _onRestart?.Invoke();
        }
    }
}