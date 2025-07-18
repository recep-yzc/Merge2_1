﻿using System.Collections.Generic;
using _Game.Development.Factory.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardLoadController : MonoBehaviour
    {
        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        public async UniTask InitializeBoardJsonData()
        {
            var json = BoardExtension.GetBoardJson();
            var boardJsonData = await UniTask.RunOnThreadPool(() => json.ConvertToBoardJsonData());
            BoardExtension.Parameters.BoardJsonData = boardJsonData;
        }

        public UniTask InitializeBoard()
        {
            BoardExtension.Parameters.GridDataList = new List<GridData>();

            CreateGrid();
            FetchGridNeighbors();
            return default;
        }

        private void CreateGrid()
        {
            foreach (var itemSaveData in BoardExtension.Parameters.BoardJsonData.ItemSaveDataList)
            {
                var gridData = CreateGridDataByItemSaveData(itemSaveData);
                if (gridData != null) BoardExtension.Parameters.GridDataList.Add(gridData);
            }
        }

        private static void FetchGridNeighbors()
        {
            foreach (var gridData in BoardExtension.Parameters.GridDataList)
            {
                var gridDataArray = BoardExtension.GetGridDataNeighborArrayByCoordinate(gridData.Coordinate);
                gridData.CopyNeighborGridData(gridDataArray);
            }
        }

        private GridData CreateGridDataByItemSaveData(ItemSaveData itemSaveData)
        {
            var createItemFunc = ItemFactory.CreateItemByItemId[itemSaveData.itemId];

            var itemDataSo = _allItemDataSo.GetItemDataSoByItemSaveData(itemSaveData);
            var itemGameObject = createItemFunc.Invoke(itemSaveData);

            return new GridData(itemSaveData.coordinate.ToVector2(), itemGameObject, itemDataSo);
        }
    }
}