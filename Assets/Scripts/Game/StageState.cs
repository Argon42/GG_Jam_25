using System.Collections.Generic;

namespace ZeroStats.Game
{
    public record StageState
    {
        public GameStage Current { get; }
        public int Day { get; set; }
        public HashSet<StageModificator> AppliedModificators { get; }
        public List<Card> AdditionalCards { get; } = new();
        public int HandSize { get; set; }


        public StageState(GameStage current, int dayValue, int handSize, HashSet<StageModificator> appliedModificators)
        {
            Current = current;
            Day = dayValue;
            HandSize = handSize;
            AppliedModificators = appliedModificators;
        }

        public void AppendCard(Card card)
        {
            AdditionalCards.Add(card);
        }
    }
}