using System;
using UnityEngine;
using UnityEngine.UI;

namespace ZeroStats.Game.Ui.StartGame
{
    public class StartGameScreen : GameScreen
    {
        [SerializeField] private Button startButton = default!;
        private Action? _onStartPress;

        private void Awake()
        {
            startButton.onClick.AddListener(OnStartClick);
        }

        public void Show(Action onStartPress)
        {
            _onStartPress = onStartPress;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnStartClick()
        {
            _onStartPress?.Invoke();
        }
    }
}