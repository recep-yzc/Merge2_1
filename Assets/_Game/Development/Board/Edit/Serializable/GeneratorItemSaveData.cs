using System;
using _Game.Development.Extension.Serializable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class GeneratorItemSaveData : ItemSaveData
    {
        public string lastUsingDate;

        public GeneratorItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId,
            string lastUsingDate) : base(coordinate, level, itemId, specialId)
        {
            this.lastUsingDate = lastUsingDate;
        }
    }
}