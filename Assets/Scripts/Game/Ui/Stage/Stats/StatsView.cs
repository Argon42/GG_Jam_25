using R3;
using UnityEngine;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Game.Ui.Stage.Stats
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField] private Player player = default!;
        [SerializeField] private StatView statViewPrefab = default!;
        [SerializeField] private RectTransform statsContainer = default!;

        private void Awake()
        {
            CreateStat(player.Stat1, StatType.First, Player.MaxStatValueAbs);
            CreateStat(player.Stat2, StatType.Second, Player.MaxStatValueAbs);
            CreateStat(player.Stat3, StatType.Third, Player.MaxStatValueAbs);
            CreateStat(player.Stat4, StatType.Fourth, Player.MaxStatValueAbs);
        }

        private void CreateStat(ReadOnlyReactiveProperty<int> stat, StatType type, int maxValue)
        {
            var statView = Instantiate(statViewPrefab, statsContainer);
            statView.SetStat(stat, maxValue, type);
        }
    }
}