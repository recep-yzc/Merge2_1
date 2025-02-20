using _Game.Development.Controller.Board;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Object.Grid
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
            var boardJsonData = boardEditController.GetBoardJsonData();
            if (boardJsonData == null) return;

            var row = (float)boardJsonData.rows;
            var column = (float)boardJsonData.columns;

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