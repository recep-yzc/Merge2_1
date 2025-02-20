using System;
using _Game.Development.Extension.Serializable;

namespace _Game.Development.Board.Edit.Serializable
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