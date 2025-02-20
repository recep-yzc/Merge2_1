using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardTransferController : MonoBehaviour
    {
        [Inject] private BoardMergeController _boardMergeController;

        private void SwapGridDataProperties(GridData gridData, GridData newGridData)
        {
            (newGridData.gameObject, gridData.gameObject) = (gridData.gameObject, newGridData.gameObject);
            (newGridData.itemDataSo, gridData.itemDataSo) = (gridData.itemDataSo, newGridData.itemDataSo);
        }

        public async UniTask<bool> TryTransfer(GridData gridData, GridData clickedGridData)
        {
            if (clickedGridData is null)
            {
                return false;
            }

            if (gridData is null)
            {
                await clickedGridData.gameObject.transform.DOMove(clickedGridData.coordinate, 0.3f)
                    .AsyncWaitForCompletion();
                return false;
            }

            var checkIsSelf = gridData.coordinate == clickedGridData.coordinate;
            if (checkIsSelf)
            {
                await clickedGridData.gameObject.transform.DOMove(clickedGridData.coordinate, 0.3f)
                    .AsyncWaitForCompletion();
                return false;
            }

            var checkIsMaxLevel = gridData.itemDataSo.nextItemDataSo == null;
            var checkIsSameItemType = gridData.itemDataSo.itemType == clickedGridData.itemDataSo.itemType;
            var checkIsSameSpecialId = gridData.itemDataSo.GetSpecialId() == clickedGridData.itemDataSo.GetSpecialId();
            var checkIsSameLevel = gridData.itemDataSo.level == clickedGridData.itemDataSo.level;

            if (!checkIsMaxLevel && checkIsSameItemType && checkIsSameSpecialId && checkIsSameLevel)
            {
                var merged = _boardMergeController.TryMerge(gridData, clickedGridData);
                if (merged) return true;
            }

            if (gridData.gameObject is not null)
            {
                SwapGridDataProperties(gridData, clickedGridData);

                gridData.gameObject?.transform.DOMove(gridData.coordinate, 0.3f);
                clickedGridData.gameObject?.transform.DOMove(clickedGridData.coordinate, 0.3f);
                return true;
            }

            SwapGridDataProperties(gridData, clickedGridData);
            clickedGridData.gameObject?.transform.DOMove(gridData.coordinate, 0.3f);
            gridData.gameObject?.transform.DOMove(gridData.coordinate, 0.3f);
            return true;
        }
    }
}