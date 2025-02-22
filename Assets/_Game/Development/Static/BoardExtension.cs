using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Development.Enum.Grid;
using _Game.Development.Serializable.Board;
using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace _Game.Development.Static
{
    public static class BoardExtension
    {
        public static string JsonPath => Application.dataPath + "/Board.json";
        private static readonly Random Random = new();

        public static GridData GetGridDataByCoordinate(Vector2 coordinate)
        {
            return Statics.GridDataList.FirstOrDefault(tileData =>
                VectorExtension.CheckOverlapWithDot(tileData.BottomLeft, tileData.TopRight, coordinate));
        }

        public static GridData[] GetGridDataNeighborArrayByCoordinate(Vector2 coordinate)
        {
            var directionIdArray = EnumExtension.ToArray<DirectionId>();
            var gridDataArray = new GridData[directionIdArray.Length];

            for (var i = 0; i < directionIdArray.Length; i++)
            {
                var directionId = directionIdArray[i];
                var newCoordinate = coordinate + directionId.DirectionToVector();

                gridDataArray[i] = GetGridDataByCoordinate(newCoordinate);
            }

            return gridDataArray;
        }

        public static T GetRandomItem<T>(this IEnumerable<T> enumerable)
        {
            using var enumerator = enumerable.GetEnumerator();
            
            var selected = enumerator.Current;
            var count = 1;

            while (enumerator.MoveNext())
            {
                count++;
                if (Random.Next(count) == 0)
                {
                    selected = enumerator.Current;
                }
            }

            return selected;
        }

        public static class Statics
        {
            public static List<GridData> GridDataList { get; set; }
            public static BoardJsonData BoardJsonData { get; set; }
            public static bool IsBoardInitialized { get; set; }
        }

        public abstract class Selector
        {
            public static Action<bool> RequestChangeVisibility { get; set; }
            public static Action<Vector2> RequestSetPosition { get; set; }
            public static Func<UniTask> RequestScaleUpDown { get; set; }
        }

        #region PlayerPrefs

        public static void SaveBoardJson(string json)
        {
            PlayerPrefs.SetString("BoardJsonData", json);
        }

        public static string GetBoardJson()
        {
            return !PlayerPrefs.HasKey("BoardJsonData")
                ? File.ReadAllText(JsonPath)
                : PlayerPrefs.GetString("BoardJsonData");
        }

        #endregion
    }
}