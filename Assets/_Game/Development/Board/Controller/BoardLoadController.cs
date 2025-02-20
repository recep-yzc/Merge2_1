using _Game.Development.Extension.Static;
using _Game.Development.Item;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Board.Controller
{
    public class BoardLoadController : MonoBehaviour
    {
        public void FetchLevelData()
        {
            //BoardGlobalValues.GridDataList = _levelDataSo.gridInspectorDataList;
        }

        public async UniTask Create()
        {
            /*foreach (var gridData in BoardGlobalValues.GridDataList)
            {
                var isExist = ItemFactory.CreateItemByCategoryType.TryGetValue(gridData.ItemDataSo.itemType.ToInt(), out var func);
                if (!isExist) continue;

                gridData.SetNeighborGridData(BoardExtension.GetGridDataNeighborArrayByCoordinate(gridData.Coordinate));
                gridData.GameObject = func.Invoke(gridData);
            }*/

            await UniTask.DelayFrame(1);
        }
    }
}