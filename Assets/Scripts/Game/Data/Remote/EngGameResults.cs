using System;
using System.Linq;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Game.Data.Remote
{
    [Serializable]
    public class EngGameResults
    {
        public int id;
        public string text;
        public string iconPath;
        public StatType[] requiredStats;
        public int[] requiredValues;
        public int minDay;

        public static EngGameResults Parse(string?[] collumns) => new()
        {
            id = GoogleSheetLoader.ParseInt(collumns[0]),
            text = collumns[1] ?? string.Empty,
            iconPath = collumns[2] ?? "placeholder",
            requiredStats = collumns[3]?.Split(',').Select(s => (StatType)GoogleSheetLoader.ParseInt(s)).ToArray() ??
                            Array.Empty<StatType>(),
            requiredValues = Array.ConvertAll(collumns[4]?.Split(',') ?? Array.Empty<string>(), GoogleSheetLoader.ParseInt),
            minDay = GoogleSheetLoader.ParseInt(collumns[5]),
        };
    }
}