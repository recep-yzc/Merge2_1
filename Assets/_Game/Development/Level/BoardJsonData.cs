using System;
using System.Collections.Generic;

namespace _Game.Development.Level
{
    [Serializable]
    public record BoardJsonData
    {
        public int rows;
        public int columns;
        public List<GridJsonData> gridJsonDataList;

        public BoardJsonData(int rows, int columns, List<GridJsonData> gridJsonDataList)
        {
            this.rows = rows;
            this.columns = columns;
            this.gridJsonDataList = gridJsonDataList;
        }
    }
}