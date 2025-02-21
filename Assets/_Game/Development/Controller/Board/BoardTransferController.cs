using _Game.Development.Interface.Ability;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardTransferController : MonoBehaviour
    {
        #region Parameters

        [Inject] private BoardMergeController _boardMergeController;
        [Inject] private MoveDataSo _moveDataSo;

        #endregion

        private void SwapGridDataProperties(GridData gridData, GridData newGridData)
        {
            (newGridData.gameObject, gridData.gameObject) = (gridData.gameObject, newGridData.gameObject);
            (newGridData.itemDataSo, gridData.itemDataSo) = (gridData.itemDataSo, newGridData.itemDataSo);
        }

        public async UniTask<bool> TryTransfer(GridData targetGridData, GridData clickedGridData)
        {
            if (clickedGridData?.gameObject is null) return false;
            var iClickedMove = clickedGridData.GetComponent<IMove>();

            if (targetGridData is null)
            {
                await iClickedMove.MoveAsync(clickedGridData.coordinate, _moveDataSo);
                return false;
            }

            var iMove = targetGridData.gameObject != null ? targetGridData.GetComponent<IMove>() : null;

            var checkIsMaxLevel = targetGridData.itemDataSo.nextItemDataSo == null;
            var checkIsSameItemType = targetGridData.itemDataSo.itemType == clickedGridData.itemDataSo.itemType;
            var checkIsSameSpecialId = targetGridData.itemDataSo.GetSpecialId() == clickedGridData.itemDataSo.GetSpecialId();
            var checkIsSameLevel = targetGridData.itemDataSo.level == clickedGridData.itemDataSo.level;
            var isSameGrid = targetGridData.coordinate == clickedGridData.coordinate;
            
            if (!checkIsMaxLevel && checkIsSameItemType && checkIsSameSpecialId && checkIsSameLevel && !isSameGrid)
            {
                var merged = _boardMergeController.TryMerge(targetGridData, clickedGridData);
                if (merged) return true;
            }

            SwapGridDataProperties(targetGridData, clickedGridData);
            iMove?.MoveAsync(clickedGridData.coordinate, _moveDataSo).Forget();
            iClickedMove?.MoveAsync(targetGridData.coordinate, _moveDataSo).Forget();

            return false;
        }
    }
}