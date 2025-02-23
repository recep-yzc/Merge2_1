using _Game.Development.Enum.Board;
using _Game.Development.Interface.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardInputController : MonoBehaviour
    {
        private void MouseDownRequest(Vector2 position)
        {
            _firstClickedPosition = _mainCamera.GetCameraPosition();
            _sourceGridDataData = BoardExtension.GetGridDataByCoordinate(_firstClickedPosition);

            if (_sourceGridDataData?.item == null)
            {
                ResetSelection();
                return;
            }

            HandleSelection(_sourceGridDataData);
        }

        private void HandleSelection(GridData sourceGridDataData)
        {
            var clickable = sourceGridDataData.GetComponent<IClickable>();
            if (clickable == null)
            {
                BoardExtension.Selector.VisibilityRequest.Invoke(false);
                return;
            }

            _isDoubleClick = _sourceGridDataData == sourceGridDataData;

            BoardExtension.Selector.SetPositionRequest.Invoke(sourceGridDataData.Coordinate);
            BoardExtension.Selector.VisibilityRequest.Invoke(true);

            clickable.MouseDown();
            _draggable = sourceGridDataData.GetComponent<IDraggable>();
        }

        private void MouseDragRequest(Vector2 position)
        {
            var cameraPosition = _mainCamera.GetCameraPosition();

            if (_isDragActive)
            {
                _draggable?.Drag(cameraPosition);
                return;
            }

            if (!(Vector2.Distance(_firstClickedPosition, cameraPosition) > _moveThreshold)) return;

            _isDragActive = true;
            BoardExtension.Selector.VisibilityRequest.Invoke(false);
        }

        private void ResetSelection()
        {
            _sourceGridDataData = null;
            _isDoubleClick = false;
            BoardExtension.Selector.VisibilityRequest.Invoke(false);
        }

        private void MouseUpRequest(Vector2 vector2)
        {
            var sourceGridData = _sourceGridDataData;
            if (sourceGridData?.item is null) return;

            if (!TryHandleEmptyOrNonClickable(sourceGridData)) return;

            var targetGridData = BoardExtension.GetGridDataByCoordinate(_mainCamera.GetCameraPosition());
            if (targetGridData is null)
            {
                HandleMouseUpOnEmptyGrid(sourceGridData);
                return;
            }

            if (sourceGridData.Coordinate == targetGridData.Coordinate)
            {
                HandleMouseUpOnSameGrid(sourceGridData);
                return;
            }

            HandleGridInteraction(sourceGridData, targetGridData);
        }

        private bool TryHandleEmptyOrNonClickable(GridData sourceGridData)
        {
            var iClickable = sourceGridData.GetComponent<IClickable>();
            if (iClickable is not null) return true;

            BoardExtension.Selector.VisibilityRequest.Invoke(false);
            return false;
        }

        private void HandleMouseUpOnEmptyGrid(GridData sourceGridData)
        {
            _boardTransferController.TryTransfer(TransferAction.Move, sourceGridData);
            FinalizeMouseUp(sourceGridData);
        }

        private void HandleMouseUpOnSameGrid(GridData sourceGridData)
        {
            BoardExtension.Selector.SetPositionRequest.Invoke(sourceGridData.Coordinate);
            BoardExtension.Selector.VisibilityRequest.Invoke(true);
            BoardExtension.Selector.ScaleUpDownRequest.Invoke();

            if (!_isDragActive && _isDoubleClick)
                _boardGenerateController.TryGenerate(sourceGridData);
            else
                _boardTransferController.TryTransfer(TransferAction.Move, sourceGridData);

            _boardScaleUpDownController.TryScaleUpDown(sourceGridData);
            FinalizeMouseUp(sourceGridData);
        }

        private void HandleGridInteraction(GridData sourceGridData, GridData targetGridData)
        {
            if (ShouldSwapItems(sourceGridData, targetGridData))
                _boardTransferController.TryTransfer(TransferAction.Swap, sourceGridData, targetGridData);
            else
                _boardMergeController.Merge(sourceGridData, targetGridData);

            BoardExtension.Selector.SetPositionRequest.Invoke(targetGridData.Coordinate);
            BoardExtension.Selector.VisibilityRequest.Invoke(true);

            _boardScaleUpDownController.TryScaleUpDown(targetGridData);
            FinalizeMouseUp(targetGridData);
        }

        private bool ShouldSwapItems(GridData sourceGridData, GridData targetGridData)
        {
            var sourceItemDataSo = sourceGridData.itemDataSo;
            var targetItemDataSo = targetGridData.itemDataSo;

            var differentTypes = sourceItemDataSo.itemType != targetItemDataSo.itemType;
            var differentSpecialId = sourceItemDataSo.GetSpecialId() != targetItemDataSo.GetSpecialId();
            var differentLevels = sourceItemDataSo.level != targetItemDataSo.level;
            var targetCannotEvolve = targetItemDataSo.nextItemDataSo == null;

            return differentTypes || differentSpecialId || differentLevels || targetCannotEvolve;
        }

        private void FinalizeMouseUp(GridData gridData)
        {
            gridData.GetComponent<IClickable>()?.MouseUp();
            _isDragActive = false;
            _draggable = null;
        }


        #region Unity Action

        private void OnEnable()
        {
            InputExtension.MouseUpRequest += MouseUpRequest;
            InputExtension.MouseDragRequest += MouseDragRequest;
            InputExtension.MouseDownRequest += MouseDownRequest;
        }

        private void OnDisable()
        {
            InputExtension.MouseUpRequest -= MouseUpRequest;
            InputExtension.MouseDragRequest -= MouseDragRequest;
            InputExtension.MouseDownRequest -= MouseDownRequest;
        }

        #endregion

        #region Parameters

        private IDraggable _draggable;
        private bool _isDragActive;
        private Vector2 _firstClickedPosition;

        private GridData _sourceGridDataData;
        private bool _isDoubleClick;

        private readonly float _moveThreshold = 0.1f;

        [Inject] private Camera _mainCamera;

        [Inject] private BoardTransferController _boardTransferController;
        [Inject] private BoardScaleUpDownController _boardScaleUpDownController;
        [Inject] private BoardGenerateController _boardGenerateController;
        [Inject] private BoardMergeController _boardMergeController;
        [Inject] private BoardSaveController _boardSaveController;

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion
    }
}