using System;
using System.Collections.Generic;
using _Game.Development.Serializable.Item;

namespace _Game.Development.Serializable.Board
{
    [Serializable]
    public class BoardJsonData
    {
        public int rows;
        public int columns;
        public List<ItemSaveData> itemSaveDataList;

        public BoardJsonData(int rows, int columns, List<ItemSaveData> itemSaveDataList)
        {
            this.rows = rows;
            this.columns = columns;
            this.itemSaveDataList = itemSaveDataList;
        }
    }
}