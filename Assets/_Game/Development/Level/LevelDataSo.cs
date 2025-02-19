using System;
using System.Collections.Generic;
using System.Reflection;
using _Game.Development.Extension;
using _Game.Development.Item;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Game.Development.Level
{
    [CreateAssetMenu(fileName = "LevelDataSo", menuName = "Game/Level/Data")]
    public class LevelDataSo : ScriptableObjectInstaller<LevelDataSo>
    {
        [TextArea] public string json;

        public ItemDataSo emptyItemDataSo;

        [MinValue(2)] [MaxValue(10)] public int rows;
        [MinValue(2)] [MaxValue(10)] public int columns;

        public List<GridData> gridDataList = new();

        [Button]
        public void GenerateGridData()
        {
            CreateGridDataList();
        }

        [Button]
        public void GenerateJson()
        {
            List<GridJsonData> gridJsonData = new();
            foreach (var gridData in gridDataList)
            {
                var itemType = gridData.itemDataSo.itemType;
                var coordinate = gridData.coordinate;

                var uniqueId = gridData.itemDataSo.uniqueId;
                var itemId = itemType.ToInt();
                var specialId = gridData.itemDataSo.GetSpecialId();

                gridJsonData.Add(new GridJsonData(coordinate, uniqueId, specialId, itemId));
            }

            var levelJsonData = new LevelJsonData(rows, columns, gridJsonData);
            json = JsonUtility.ToJson(levelJsonData);
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
                return new GridData(new Vector2(x, y) - offset, emptyItemDataSo);
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