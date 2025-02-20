using System.Collections.Generic;
using System.Linq;
using _Game.Development.Enum.Grid;
using _Game.Development.Serializable.Board;
using _Game.Development.Serializable.Grid;
using UnityEngine;

namespace _Game.Development.Static
{
    public static class BoardExtension
    {
        public static string JsonPath => Application.dataPath + "/Board.json";
        public static List<GridData> LiveGridDataList { get; } = new();
        public static BoardJsonData BoardJsonData { get; set; }

        public static GridData GetGridDataByCoordinate(Vector2 coordinate)
        {
            return LiveGridDataList.FirstOrDefault(tileData =>
                VectorExtension.CheckOverlapWithDot(tileData.BottomLeft, tileData.TopRight, coordinate));
        }

        public static GridData[] GetGridDataNeighborArrayByCoordinate(Vector2 coordinate)
        {
            var directionIdArray = EnumExtension.ToArray<DirectionId>();
            var gridDataArray = new GridData[directionIdArray.Length];

            for (var i = 0; i < directionIdArray.Length; i++)
            {
                var direction = directionIdArray[i];
                var newCoordinate = coordinate + direction.DirectionToVector();

                gridDataArray[i] = GetGridDataByCoordinate(newCoordinate);
            }

            return gridDataArray;
        }
    }
}