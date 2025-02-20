using System.IO;
using _Game.Development.Factory.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
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

        public void InitBoardJsonData()
        {
            var json = File.ReadAllText(BoardExtension.JsonPath);
            BoardExtension.BoardJsonData = json.ConvertToBoardJsonData();
        }

        public async UniTask CreateBoard()
        {
            foreach (var itemSaveData in BoardExtension.BoardJsonData.itemSaveDataList)
            {
                var itemId = itemSaveData.itemId;

                var isExist = ItemFactory.CreateItemByItemId.TryGetValue(itemId, out var func);
                if (!isExist) continue;

                var specialId = itemSaveData.specialId;
                var level = itemSaveData.level;

                var neighborGridDataList =
                    BoardExtension.GetGridDataNeighborArrayByCoordinate(itemSaveData.coordinate.ToVector2());
                var itemDataSo = _allItemDataSo.GetItemDataByIds(itemId, specialId, level);
                var itemGameObject = func.Invoke(itemSaveData);

                var gridData = new GridData(itemSaveData.coordinate.ToVector2(), itemGameObject, itemDataSo,
                    neighborGridDataList);

                BoardExtension.LiveGridDataList.Add(gridData);
            }

            await UniTask.DelayFrame(1);
        }
    }
}