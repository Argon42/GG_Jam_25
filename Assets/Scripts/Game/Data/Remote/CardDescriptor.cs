using System;
using ZeroStats.Game.Data.Enums;

namespace ZeroStats.Game.Data.Remote
{
    [Serializable]
    public class CardDescriptor
    {
        public int CardId;
        public int Weight;
        public GameStage[] NotApplicableStages;
        public int StatNumber;
        public int MinStatForUse;
        public int MaxStatForUse;
    }
}