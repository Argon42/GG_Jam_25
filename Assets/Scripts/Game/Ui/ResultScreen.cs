using System;
using UnityEngine;
using UnityEngine.UI;

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

        public void Show(Action onRestart)
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