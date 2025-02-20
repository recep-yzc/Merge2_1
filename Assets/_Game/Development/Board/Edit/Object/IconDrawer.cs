using _Game.Development.Board.Edit.Controller;
using _Game.Development.Extension.Static;
using UnityEngine;

namespace _Game.Development.Board.Edit.Object
{
    public class IconDrawer : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BoardEditController boardEditController;

        #region Unity Actions

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            DrawIcon();
            DrawMouseIcon();
        }

        #endregion

        private void DrawIcon()
        {
            var levelDataSo = boardEditController.GetLevelDataSo();
            foreach (var gridInspectorData in levelDataSo.gridInspectorDataList)
            {
                if (gridInspectorData.itemDataSo == null) continue;

                var iconPath = gridInspectorData.itemDataSo.GetIconPath();
                var coordinate = gridInspectorData.coordinate;

                Gizmos.DrawIcon(coordinate.ToVector2(), iconPath, true);
                Gizmos.DrawWireCube(coordinate.ToVector2(), VectorExtension.Size);
            }
        }

        private void DrawMouseIcon()
        {
            var itemDataSo = boardEditController.GetSelectedGridItemDataSo();
            if (itemDataSo == null) return;

            var coordinate = boardEditController.GetCamera().GetCoordinate();
            var iconPath = itemDataSo.GetIconPath();

            Gizmos.DrawIcon(coordinate, iconPath, true);
            Gizmos.DrawWireCube(coordinate, VectorExtension.Size);
        }
    }
}