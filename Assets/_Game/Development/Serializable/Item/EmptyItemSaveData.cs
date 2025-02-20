using System;
using _Game.Development.Serializable.Grid;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class EmptyItemSaveData : ItemSaveData
    {
        public EmptyItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId) : base(
            coordinate, level, itemId, specialId)
        {
        }
    }
}