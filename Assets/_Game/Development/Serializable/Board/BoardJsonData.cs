using System;
using System.Collections.Generic;
using _Game.Development.Serializable.Item;

namespace _Game.Development.Serializable.Board
{
    [Serializable]
    public record BoardJsonData
    {
        public readonly int Columns;
        public readonly List<ItemSaveData> ItemSaveDataList;
        public readonly int Rows;

        public BoardJsonData(int rows, int columns, List<ItemSaveData> itemSaveDataList)
        {
            Rows = rows;
            Columns = columns;
            ItemSaveDataList = itemSaveDataList;
        }
    }
}