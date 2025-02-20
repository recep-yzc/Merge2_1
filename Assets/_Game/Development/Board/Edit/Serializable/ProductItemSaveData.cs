using System;
using _Game.Development.Extension.Serializable;

namespace _Game.Development.Board.Edit.Serializable
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