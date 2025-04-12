using System;
using R3;
using TMPro;
using UnityEngine;

namespace ZeroStats.Game
{
    public class StatsView : MonoBehaviour
    {
        [SerializeField] private Player player = default!;
        [SerializeField] private TMP_Text stat1 = default!;
        [SerializeField] private TMP_Text stat2 = default!;
        [SerializeField] private TMP_Text stat3 = default!;
        [SerializeField] private TMP_Text stat4 = default!;

        private void Awake()
        {
            player.Stat1.Subscribe(i => stat1.text = i.ToString()).AddTo(this);
            player.Stat2.Subscribe(i => stat2.text = i.ToString()).AddTo(this);
            player.Stat3.Subscribe(i => stat3.text = i.ToString()).AddTo(this);
            player.Stat4.Subscribe(i => stat4.text = i.ToString()).AddTo(this);
        }
    }
}