﻿using _Game.Development.Controller.Board;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Object.Grid
{
    public class IconDrawer : MonoBehaviour
    {
        #region Parameters

        [Inject] private BoardEditController _boardEditController;

        #endregion

        #region Unity Action

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            DrawIcon();
            DrawMouseIcon();
        }

        #endregion

        private void DrawIcon()
        {
            var boardJsonData = _boardEditController.GetBoardJsonData();
            if (boardJsonData == null) return;

            foreach (var itemSaveData in boardJsonData.ItemSaveDataList)
            {
                var itemDataSo = _boardEditController.GetItemDataSoByItemSaveData(itemSaveData);

                var iconPath = itemDataSo.GetIconPath();
                var coordinate = itemSaveData.coordinate.ToVector2();

                Gizmos.DrawIcon(coordinate, iconPath, true);
                Gizmos.DrawWireCube(coordinate, VectorExtension.Parameters.Size);
            }
        }

        private void DrawMouseIcon()
        {
            var itemDataSo = _boardEditController.GetSelectedGridItemDataSo();
            if (itemDataSo == null) return;

            var coordinate = _boardEditController.GetCamera().GetCameraPosition();
            var iconPath = itemDataSo.GetIconPath();

            Gizmos.DrawIcon(coordinate, iconPath, true);
            Gizmos.DrawWireCube(coordinate, VectorExtension.Parameters.Size);
        }
    }
}