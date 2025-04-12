﻿using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ZeroStats.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private StartGameScreen startGameScreen = default!;
        [SerializeField] private StageScreen stageScreen = default!;
        [SerializeField] private StagePreloaderScreen stagePreloaderScreen = default!;
        [SerializeField] private ResultScreen resultScreen = default!;
        [SerializeField] private BackgroundWorld backgroundWorld = default!;
        [SerializeField] private Player player = default!;

        private void Start()
        {
            InitializeAllScreens(startGameScreen, stageScreen, stagePreloaderScreen, resultScreen);

            startGameScreen.Show(() => StartGame(() => startGameScreen.Hide()).Forget());
            backgroundWorld.Show(GameStage.StartGame);
            player.StartNewGame();
        }

        private async UniTaskVoid StartGame(Action hideAction)
        {
            var nextStage = player.GetNextStage();
            await stagePreloaderScreen.Show(nextStage, () =>
            {
                hideAction();
                stageScreen.Show(player.GetCards(nextStage), card => OnSelectCard(card).Forget());
                backgroundWorld.Show(nextStage.Current);
            });
        }

        private async UniTaskVoid OnSelectCard(Card selectedCard)
        {
            player.ApplyEffect(selectedCard);
            await stageScreen.ApplyEffect(selectedCard);
            if (player.IsLoose())
            {
                backgroundWorld.Show(GameStage.Result);
                resultScreen.Show(OnRestart);
                return;
            }

            var stageState = player.GetNextStage();
            await stagePreloaderScreen.Show(stageState, () =>
            {
                stageScreen.Show(player.GetCards(stageState), card => OnSelectCard(card).Forget());
                backgroundWorld.Show(GameStage.Day);
            });
        }

        private void OnRestart()
        {
            player.StartNewGame();
            StartGame(() => resultScreen.Hide()).Forget();
        }

        private void InitializeAllScreens(params GameScreen[] screens)
        {
            foreach (var screen in screens)
            {
                screen.gameObject.SetActive(false);
                ((RectTransform)screen.transform).anchoredPosition = Vector2.zero;
            }
        }
    }
}