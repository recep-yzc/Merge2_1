using _Game.Development.Static;
using UnityEditor;
using UnityEngine;

namespace _Game.Development.Object.Grid
{
    public class GridDataDrawer : MonoBehaviour
    {
        #region Unity Actions

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            DrawGrid();
        }

        #endregion

        private void DrawGrid()
        {
            if (BoardExtension.Statics.GridDataList is null) return;

            var style = new GUIStyle
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter
            };

            foreach (var gridData in BoardExtension.Statics.GridDataList)
                Handles.Label(gridData.coordinate, gridData.itemDataSo.name + "\n" +
                                                   gridData.itemDataSo.itemType + "\n" + gridData.item.name + "\n" +
                                                   gridData.itemDataSo.GetSpecialId() + "\n" +
                                                   gridData.itemDataSo.level, style);
        }
    }
}