using System;
using _Game.Development.Static;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class GeneratorItemSaveData : ItemSaveData
    {
        public string lastUsingDate;

        public GeneratorItemSaveData(VectorExtension.JsonVector2 coordinate, int level, int itemId, int specialId,
            string lastUsingDate) : base(coordinate, level, itemId, specialId)
        {
            this.lastUsingDate = lastUsingDate;
        }
    }
}