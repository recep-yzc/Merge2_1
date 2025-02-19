using System;
using System.Collections.Generic;

namespace _Game.Development.Level
{
    [Serializable]
    public record LevelJsonData
    {
        public int rows;
        public int columns;
        public List<GridJsonData> gridJsonDataList;

        public LevelJsonData(int rows, int columns, List<GridJsonData> gridJsonDataList)
        {
            this.rows = rows;
            this.columns = columns;
            this.gridJsonDataList = gridJsonDataList;
        }
    }
}