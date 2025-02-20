using System;

namespace _Game.Development.Item.Serializable
{
    [Serializable]
    public class EmptyItemSaveData : ItemSaveData
    {
        public EmptyItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId) : base(
            coordinate,
            level, itemId, specialId)
        {
        }
    }
}