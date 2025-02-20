using System;

namespace _Game.Development.Item.Serializable
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