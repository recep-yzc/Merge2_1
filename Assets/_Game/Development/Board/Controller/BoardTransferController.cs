using _Game.Development.Grid.Serializable;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board.Controller
{
    public class BoardTransferController : MonoBehaviour
    {
        [Inject] private BoardMergeController _boardMergeController;

        private void SwapGridDataProperties(GridData gridData, GridData newGridData)
        {
            (newGridData.GameObject, gridData.GameObject) = (gridData.GameObject, newGridData.GameObject);
            (newGridData.ItemDataSo, gridData.ItemDataSo) = (gridData.ItemDataSo, newGridData.ItemDataSo);
        }

        public async UniTask<bool> TryTransfer(GridData gridData, GridData clickedGridData)
        {
            if (gridData is null)
            {
                clickedGridData.GameObject.transform.DOMove(clickedGridData.Coordinate, 0.3f);
                return false;
            }

            var checkIsMaxLevel = false;
            var checkIsSameItemType = gridData.ItemDataSo.itemType == clickedGridData.ItemDataSo.itemType;
            var checkIsSameLevel = gridData.ItemDataSo.level == clickedGridData.ItemDataSo.level;
            var checkIsSelf = gridData.Coordinate == clickedGridData.Coordinate;

            if (checkIsSelf)
            {
                clickedGridData.GameObject.transform.DOMove(clickedGridData.Coordinate, 0.3f);
                return false;
            }

            if (!checkIsMaxLevel && checkIsSameItemType && checkIsSameLevel && !checkIsSelf)
            {
                var merged = await _boardMergeController.TryMerge(gridData, clickedGridData);
                if (merged) return true;
            }
            
            if (gridData.GameObject is not null)
            {
                SwapGridDataProperties(gridData, clickedGridData);

                gridData.GameObject?.transform.DOMove(gridData.Coordinate, 0.3f);
                clickedGridData.GameObject?.transform.DOMove(clickedGridData.Coordinate, 0.3f);
                return true;
            }
            else
            {
                SwapGridDataProperties(gridData, clickedGridData);
                clickedGridData.GameObject?.transform.DOMove(gridData.Coordinate, 0.3f);
                gridData.GameObject?.transform.DOMove(gridData.Coordinate, 0.3f);
                return true;
            }
        }
    }
}