using System.Collections.Generic;
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
        public List<LevelGridData> levelGridDataList = new();

        [MinValue(2)] [MaxValue(10)] public int rows;
        [MinValue(2)] [MaxValue(10)] public int columns;

        [Button]
        public void GenerateGridData()
        {
            levelGridDataList.Clear();

            var halfOfRows = rows * 0.5f;
            var halfOfColumns = columns * 0.5f;
            var offset = new Vector2(halfOfRows, halfOfColumns) - VectorExtension.HalfSize;

            LevelGridData CreateTileData(int x, int y)
            {
                return new LevelGridData(new Vector2(x, y) - offset);
            }

            for (var x = 0; x < rows; x++)
            for (var y = 0; y < columns; y++)
                levelGridDataList.Add(CreateTileData(x, y));
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