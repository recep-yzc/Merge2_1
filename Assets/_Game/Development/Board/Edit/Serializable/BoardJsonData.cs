using System;
using System.Collections.Generic;

namespace _Game.Development.Board.Edit.Serializable
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