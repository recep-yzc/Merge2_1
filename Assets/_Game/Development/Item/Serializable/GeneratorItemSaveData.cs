using System;

namespace _Game.Development.Item.Serializable
{
    [Serializable]
    public class GeneratorItemSaveData : ItemSaveData
    {
        public string lastUsingDate;

        public GeneratorItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId,
            string lastUsingDate) : base(coordinate, level, itemId, specialId)
        {
            this.lastUsingDate = lastUsingDate;
        }
    }
}