using _Game.Development.Extension;
using _Game.Development.Item;
using _Game.Development.Level;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board
{
    public class BoardLoadController : MonoBehaviour
    {
        #region Parameters

        [Inject] private LevelDataSo _levelDataSo;

        #endregion

        public void FetchLevelData()
        {
            BoardGlobalValues.GridDataList = _levelDataSo.gridDataList;
        }

        public async UniTask Create()
        {
            foreach (var gridData in BoardGlobalValues.GridDataList)
            {
                var isExist =
                    ItemFactory.CreateItemByCategoryType.TryGetValue(gridData.itemDataSo.itemType.ToInt(),
                        out var func);
                if (!isExist) continue;

                gridData.SetNeighborGridData(BoardExtension.GetGridDataNeighborArrayByCoordinate(gridData.coordinate));
                gridData.GameObject = func.Invoke(gridData);
            }

            await UniTask.DelayFrame(1);
        }
    }
}