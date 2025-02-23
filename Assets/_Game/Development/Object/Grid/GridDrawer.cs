using _Game.Development.Controller.Board;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Object.Grid
{
    public class GridDrawer : MonoBehaviour
    {
        #region Parameters

        [Inject] private BoardEditController _boardEditController;

        #endregion

        #region Unity Action

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            DrawGrid();
        }

        #endregion

        private void DrawGrid()
        {
            var boardJsonData = _boardEditController.GetBoardJsonData();
            if (boardJsonData is null) return;

            var row = (float)boardJsonData.Rows;
            var column = (float)boardJsonData.Columns;

            var gridCenterOffset = new Vector2(-row / 2, -column / 2);
            for (var x = 0; x < row; x++)
            for (var y = 0; y < column; y++)
            {
                var gridCenter = new Vector2(x, y) + gridCenterOffset + VectorExtension.Parameters.HalfSize;
                Gizmos.DrawWireCube(gridCenter, VectorExtension.Parameters.Size);
            }
        }
    }
}