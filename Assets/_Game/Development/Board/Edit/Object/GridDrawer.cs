using _Game.Development.Board.Edit.Controller;
using _Game.Development.Extension.Static;
using UnityEngine;

namespace _Game.Development.Board.Edit.Object
{
    public class GridDrawer : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BoardEditController boardEditController;

        #region Unity Actions

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            DrawGrid();
        }

        #endregion

        private void DrawGrid()
        {
            var levelDataSo = boardEditController.GetLevelDataSo();
            var row = (float)levelDataSo.rows;
            var column = (float)levelDataSo.columns;

            var gridCenterOffset = new Vector2(-row / 2, -column / 2);
            for (var x = 0; x < row; x++)
            for (var y = 0; y < column; y++)
            {
                var gridCenter = new Vector2(x, y) + gridCenterOffset + VectorExtension.HalfSize;
                Gizmos.DrawWireCube(gridCenter, VectorExtension.Size);
            }
        }
    }
}