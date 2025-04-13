using ZeroStats.Game.Data.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZeroStats.Game.Ui
{
    public class BackgroundWorld : MonoBehaviour
    {
        [SerializeField] private RectTransform backgroundContainer = default!;
        
        [SerializeField] private List<GameObject> backgrounds = new List<GameObject>();
        
        private List<GameObject> backgroundsMorning = new List<GameObject>();
        private List<GameObject> backgroundsDay = new List<GameObject>();
        private List<GameObject> backgroundsEvening = new List<GameObject>();
        private List<GameObject> backgroundsNight = new List<GameObject>();
        
        private void Awake()
        {
            foreach (var background in backgrounds)
            {
                var createdBackground = CreateBackground(background);
                string nameBackground = background.name.ToLower();
                
                if (nameBackground.Contains("morning"))
                    backgroundsMorning.Add(createdBackground);
                else if (nameBackground.Contains("day"))
                    backgroundsDay.Add(createdBackground);
                else if (nameBackground.Contains("evening"))
                    backgroundsEvening.Add(createdBackground);
                else if (nameBackground.Contains("night"))
                    backgroundsNight.Add(createdBackground);
            }
        }

        private GameObject CreateBackground(GameObject background)
        {
            var backgroundView = Instantiate(background, backgroundContainer);
            backgroundView.SetActive(false);
            return backgroundView;
        }

        private void HideAllBackgrounds()
        {
            foreach (var background in backgroundsMorning) background.SetActive(false);
            foreach (var background in backgroundsDay) background.SetActive(false);
            foreach (var background in backgroundsEvening) background.SetActive(false);
            foreach (var background in backgroundsNight) background.SetActive(false);
        }
        
        public void Show(GameStage state)
        {
            HideAllBackgrounds();
            switch (state)
            {
                case GameStage.Morning:
                    backgroundsMorning[Random.Range(0, backgroundsMorning.Count-1)].SetActive(true);
                    return;
                case GameStage.Day:
                    backgroundsDay[Random.Range(0, backgroundsDay.Count-1)].SetActive(true);
                    return;
                case GameStage.Evening:
                    backgroundsEvening[Random.Range(0, backgroundsEvening.Count-1)].SetActive(true);
                    return;
                case GameStage.Night:
                    backgroundsNight[Random.Range(0, backgroundsNight.Count-1)].SetActive(true);
                    return;
                case GameStage.Result:
                    return;
                case GameStage.None:
                    return;
                case GameStage.StartGame:
                    return;
                default:
                    return;
            }
        }
    }
}