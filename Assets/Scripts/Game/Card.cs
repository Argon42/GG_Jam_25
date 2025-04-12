namespace ZeroStats.Game
{
    public class Card
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Stat1Delta { get; set; }
        public int Stat2Delta { get; set; }
        public int Stat3Delta { get; set; }
        public int Stat4Delta { get; set; }
    }
}