using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Development.Extension;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Game.Development.Level
{
    [CreateAssetMenu(fileName = "LevelDataSo", menuName = "Game/Level/Data")]
    public class LevelDataSo : ScriptableObjectInstaller<LevelDataSo>
    {
        public ItemDataSo itemDataSo;

        [Button("Load Board Json")]
        public void LoadBoardJson(TextAsset boardJson)
        {
            var boardJsonData = JsonUtility.FromJson<BoardJsonData>(boardJson.text);

            rows = boardJsonData.rows;
            columns = boardJsonData.columns;
            gridDataList.Clear();

            foreach (var gridJsonData in boardJsonData.gridJsonDataList)
            {
                var itemTypeData = itemDataSo.itemDataSoList.First(x => x.itemType.ToInt() == gridJsonData.itemId);
                var specialIdData = itemTypeData.specialIdDataList.First(x => x.specialId == gridJsonData.specialId);
                var newItemDataSo = specialIdData.itemItemDataSoList.First(x => x.level == gridJsonData.level);

                gridDataList.Add(new GridData(gridJsonData.coordinate, newItemDataSo));
            }
        }


        [MinValue(2)] [MaxValue(10)] public int rows;
        [MinValue(2)] [MaxValue(10)] public int columns;

        public List<GridData> gridDataList = new();

        [Button]
        public void GenerateGridData()
        {
            CreateGridDataList();
        }

        [Button("Generate Board Json")]
        public void GenerateBoardJson()
        {
            List<GridJsonData> gridJsonData = new();
            foreach (var gridData in gridDataList)
            {
                var itemType = gridData.itemDataSo.itemType;
                var coordinate = gridData.coordinate;

                var level = gridData.itemDataSo.level;
                var itemId = itemType.ToInt();
                var specialId = gridData.itemDataSo.GetSpecialId();

                gridJsonData.Add(new GridJsonData(coordinate, level, specialId, itemId));
            }

            var json = JsonUtility.ToJson(new BoardJsonData(rows, columns, gridJsonData), true);
            var filePath = Application.dataPath + "/Board.json";
            File.WriteAllText(filePath, json);
            AssetDatabase.SaveAssets();
        }

        private void FetchGridNeighborList()
        {
            /* BoardGlobalValues.GridDataList = gridDataList;

             foreach (var gridData in gridDataList)
             {
                 List<GridData> neighborList = new();

                 foreach (var directionId in EnumExtension.ToArray<DirectionId>())
                 {
                     var direction = directionId.DirectionToVector();
                     var coordinate = gridData.coordinate + direction;
                     var neighborGridData = BoardExtension.GetGridDataByCoordinate(coordinate);

                     if (neighborGridData == null) continue;
                     neighborList.Add(neighborGridData);
                 }

                 gridData.SetNeighborGridData(neighborList.ToList());
             }*/
        }

        private void CreateGridDataList()
        {
            gridDataList.Clear();

            var halfOfRows = rows * 0.5f;
            var halfOfColumns = columns * 0.5f;
            var offset = new Vector2(halfOfRows, halfOfColumns) - VectorExtension.HalfSize;

            GridData CreateGridData(int x, int y)
            {
                return new GridData(new Vector2(x, y) - offset, itemDataSo.itemDataSoList[0].specialIdDataList[0].itemItemDataSoList[0]);
            }

            for (var x = 0; x < rows; x++)
            for (var y = 0; y < columns; y++)
                gridDataList.Add(CreateGridData(x, y));
        }

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }

        #region Unity Action

#if UNITY_EDITOR
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif

        #endregion
    }
}