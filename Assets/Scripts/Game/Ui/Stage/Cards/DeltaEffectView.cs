using UnityEngine;
using UnityEngine.UI;
using ZeroStats.Common;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Game.Ui.Stage.Cards
{
    public class DeltaEffectView : MonoBehaviour
    {
        [SerializeField] private Image icon = default!;
        [SerializeField] private GameObject upStat = default!;
        [SerializeField] private GameObject downStat = default!;
        [SerializeField] private GameObject unknownStat = default!;

        public void Show(int delta, StatType stat, bool knowCard)
        {
            gameObject.SetActive(true);

            icon.sprite = G.LoadSprite($"Icons/{stat.ToString()}");

            icon.color = icon.sprite == null 
                ? G.LoadColor($"ColorsBars/{stat.ToString()}") 
                : Color.white;

            if (!knowCard)
            {
                unknownStat.SetActive(true);
                upStat.SetActive(false);
                downStat.SetActive(false);
                return;
            }

            switch (delta)
            {
                case > 0:
                    upStat.SetActive(true);
                    downStat.SetActive(false);
                    return;
                case < 0:
                    upStat.SetActive(false);
                    downStat.SetActive(true);
                    return;
                default:
                    upStat.SetActive(false);
                    downStat.SetActive(false);
                    Debug.LogError("delta == 0, stat: " + stat);
                    return;
            }
        }
    }
}