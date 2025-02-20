using System.IO;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Board.Edit.Static;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item;
using _Game.Development.Item.Scriptable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board.Controller
{
    public class BoardLoadController : MonoBehaviour
    {
        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        public void InitBoardJsonData()
        {
            var json = File.ReadAllText(EditGlobalValues.JsonPath);
            BoardGlobalValues.BoardJsonData = json.ConvertToBoardJsonData();
        }

        public async UniTask CreateBoard()
        {
            foreach (var itemSaveData in BoardGlobalValues.BoardJsonData.itemSaveDataList)
            {
                var isExist = ItemFactory.CreateItemByItemId.TryGetValue(itemSaveData.itemId, out var func);
                if (!isExist) continue;

                var itemGameObject = func.Invoke(itemSaveData);
                var itemDataSo = _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);
                var neighborGridDataList = BoardExtension.GetGridDataNeighborArrayByCoordinate(itemSaveData.coordinate.ToVector2());

                var gridData = new GridData
                {
                    Coordinate = itemSaveData.coordinate.ToVector2(),
                    GameObject = itemGameObject,
                    ItemDataSo = itemDataSo,
                    NeighborGridData = neighborGridDataList
                };

                BoardGlobalValues.GridDataList.Add(gridData);
            }

            await UniTask.DelayFrame(1);
        }
    }
}