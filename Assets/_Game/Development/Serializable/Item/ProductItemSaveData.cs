using System;
using _Game.Development.Serializable.Grid;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class ProductItemSaveData : ItemSaveData
    {
        public ProductItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId) : base(
            coordinate,
            level, itemId, specialId)
        {
        }
    }
}