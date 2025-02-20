using System.Linq;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Enum;
using _Game.Development.Grid.Serializable;
using UnityEngine;

namespace _Game.Development.Board.Controller
{
    public static class BoardExtension
    {
        public static GridData GetGridDataByCoordinate(Vector2 coordinate)
        {
            return BoardGlobalValues.GridDataList.FirstOrDefault(tileData =>
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