using System.Collections.Generic;
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
        public List<GridData> GridDataList = new();

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        public async UniTask InitializeBoardJsonData()
        {
            var json = BoardExtension.GetBoardJson();
            var boardJsonData = await UniTask.RunOnThreadPool(() => json.ConvertToBoardJsonData());
            BoardExtension.Statics.BoardJsonData = boardJsonData;
            await UniTask.DelayFrame(1);
        }

        public async UniTask InitializeBoard()
        {
            BoardExtension.Statics.GridDataList = new List<GridData>();

            CreateGrid();
            FetchGridNeighbors();

            await UniTask.DelayFrame(1);
        }

        private void CreateGrid()
        {
            foreach (var itemSaveData in BoardExtension.Statics.BoardJsonData.itemSaveDataList)
            {
                var gridData = CreateGridDataByItemSaveData(itemSaveData);
                if (gridData != null) BoardExtension.Statics.GridDataList.Add(gridData);
            }

            GridDataList = BoardExtension.Statics.GridDataList;
        }

        private static void FetchGridNeighbors()
        {
            foreach (var gridData in BoardExtension.Statics.GridDataList)
                gridData.CopyNeighborGridData(BoardExtension.GetGridDataNeighborArrayByCoordinate(gridData.coordinate));
        }

        private GridData CreateGridDataByItemSaveData(ItemSaveData itemSaveData)
        {
            if (!ItemFactory.CreateItemByItemId.TryGetValue(itemSaveData.itemId, out var createItemFunc))
                return null;

            var itemDataSo =
                _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);
            var itemGameObject = createItemFunc.Invoke(itemSaveData);

            return new GridData(itemSaveData.coordinate.ToVector2(), itemGameObject, itemDataSo);
        }
    }
}