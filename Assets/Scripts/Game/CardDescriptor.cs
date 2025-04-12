namespace ZeroStats.Game
{
    public class CardDescriptor
    {
        public Card Card { get; }
        public int Weight { get; }
        public GameStage[] NotApplicableStages { get; }
        public int StatNumber { get; }
        public int MinStatForUse { get; }
        public int MaxStatForUse { get; }
    }
}