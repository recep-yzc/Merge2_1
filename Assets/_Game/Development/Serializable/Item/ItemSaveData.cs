using System;
using _Game.Development.Static;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class ItemSaveData
    {
        public VectorExtension.JsonVector2 coordinate;

        public int level;
        public int specialId;
        public int itemId;

        public ItemSaveData(VectorExtension.JsonVector2 coordinate, int level, int itemId, int specialId)
        {
            this.coordinate = coordinate;

            this.level = level;
            this.specialId = specialId;
            this.itemId = itemId;
        }
    }
}