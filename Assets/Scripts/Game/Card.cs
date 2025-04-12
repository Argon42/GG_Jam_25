using System;

namespace ZeroStats.Game
{
    
    [Serializable]
    public class Card
    {
        public int Id;
        public string IconPath;
        public string Name;
        public string ResultDescription;
        public string? ResultResourcesPath;

        public int Stat1Delta;
        public int Stat2Delta;
        public int Stat3Delta;
        public int Stat4Delta;
        public int Group;
    }
}