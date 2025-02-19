using _Game.Development.Extension;
using UnityEngine;
using Zenject;

namespace _Game.Development.Level
{
    public class GridDrawer : MonoBehaviour
    {
        private void DrawGrid()
        {
            if (_levelDataSo == null) return;

            var row = (float)_levelDataSo.rows;
            var column = (float)_levelDataSo.columns;

            var gridCenterOffset = new Vector2(-row / 2, -column / 2);
            for (var x = 0; x < row; x++)
            for (var y = 0; y < column; y++)
            {
                var cellCenter = new Vector2(x, y) + gridCenterOffset + VectorExtension.HalfSize;
                Gizmos.DrawWireCube(cellCenter, VectorExtension.Size);
            }
        }

        private void DrawIcon()
        {
            if (_levelDataSo == null) return;

            foreach (var tileLevelData in _levelDataSo.levelGridDataList)
            {
                if (tileLevelData.itemDataSo == null) continue;

                var iconPath = tileLevelData.itemDataSo.GetIconPath();
                var coordinate = tileLevelData.coordinate;

                Gizmos.DrawIcon(coordinate, iconPath, true);
                Gizmos.DrawWireCube(coordinate, VectorExtension.Size);
            }
        }

        private void DrawMouseIcon()
        {
            if (_camera == null) return;

            var gridItemDataSo = LevelEditor.SelectedItemDataSo;
            if (gridItemDataSo == null) return;

            var mousePosition = Input.mousePosition;
            mousePosition.z = _camera.orthographicSize;

            var coordinate = _camera.ScreenToWorldPoint(mousePosition);
            var iconPath = gridItemDataSo.GetIconPath();

            Gizmos.DrawIcon(coordinate, iconPath, true);
            Gizmos.DrawWireCube(coordinate, VectorExtension.Size);
        }

        #region Parameters

        [Inject] private LevelDataSo _levelDataSo;
        private Camera _camera;

        #endregion

        #region Unity Actions

        private void Start()
        {
            _camera = Camera.main;
        }

        private void OnDrawGizmos()
        {
            DrawGrid();
            DrawIcon();
            DrawMouseIcon();
        }

        #endregion
    }
}