using System;
using _Game.Development.Extension.Serializable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class ItemSaveData
    {
        public SerializableVector2 coordinate;

        public int level;
        public int specialId;
        public int itemId;

        public ItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId)
        {
            this.coordinate = coordinate;

            this.level = level;
            this.specialId = specialId;
            this.itemId = itemId;
        }
    }
}