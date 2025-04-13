using System;
using System.Collections.Generic;

namespace ZeroStats.Game.Data.Remote
{
    [Serializable]
    public class CardDatabase
    {
        public Card[] cards = default!;
        public CardDescriptor[] cardDescriptors = default!;
        public ColorData[] colors = default!;
        public ParametersData parameters = default!;
        public EngGameResults[] engResults = default!;
    }
}