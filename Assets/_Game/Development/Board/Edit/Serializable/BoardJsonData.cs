using System;
using System.Collections.Generic;
using _Game.Development.Item.Serializable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class BoardJsonData
    {
        public int rows;
        public int columns;
        public List<ItemSaveData> gridJsonDataList;

        public BoardJsonData(int rows, int columns, List<ItemSaveData> gridJsonDataList)
        {
            this.rows = rows;
            this.columns = columns;
            this.gridJsonDataList = gridJsonDataList;
        }
    }
}