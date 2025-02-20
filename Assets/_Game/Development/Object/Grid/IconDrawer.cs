using _Game.Development.Controller.Board;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Object.Grid
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
            var boardJsonData = boardEditController.GetBoardJsonData();
            if (boardJsonData == null) return;

            foreach (var itemSaveData in boardJsonData.itemSaveDataList)
            {
                var itemDataSo = boardEditController.GetItemDataSoByItemSaveData(itemSaveData);

                var iconPath = itemDataSo.GetIconPath();
                var coordinate = itemSaveData.coordinate;

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