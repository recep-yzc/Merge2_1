using System;
using _Game.Development.Static;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class ProductItemSaveData : ItemSaveData
    {
        public ProductItemSaveData(VectorExtension.JsonVector2 coordinate, int level, int itemId, int specialId) : base(
            coordinate, level,
            itemId, specialId)
        {
        }
    }
}