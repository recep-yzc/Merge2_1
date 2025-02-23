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
        private void MouseDownRequested(Vector2 vector2)
        {
            _firstClickedPosition = _mainCamera.GetCameraPosition();
            var gridData = BoardExtension.GetGridDataByCoordinate(_firstClickedPosition);

            if (gridData?.item is null)
            {
                _mouseDownGridData = null;
                _isDoubleClick = false;

                BoardExtension.Selector.VisibilityRequest.Invoke(false);
                return;
            }

            _mouseDownGridData = gridData;

            var iClickable = gridData.GetComponent<IClickable>();
            if (iClickable is null)
            {
                BoardExtension.Selector.VisibilityRequest.Invoke(false);
                return;
            }

            _isDoubleClick = _mouseDownGridData == gridData;

            BoardExtension.Selector.SetPositionRequest.Invoke(gridData.Coordinate);
            BoardExtension.Selector.VisibilityRequest.Invoke(true);

            iClickable.MouseDown();
            _draggable = gridData.GetComponent<IDraggable>();
        }

        private void MouseDragRequested(Vector2 vector2)
        {
            var cameraPosition = _mainCamera.GetCameraPosition();

            if (_isDragActive)
            {
                _draggable?.Drag(cameraPosition);
                return;
            }

            var magnitude = (_firstClickedPosition - cameraPosition).magnitude;
            if (magnitude > _moveThreshold)
            {
                _isDragActive = true;
                BoardExtension.Selector.VisibilityRequest.Invoke(false);
            }
        }

        private void MouseUpRequested(Vector2 vector2)
        {
            var mouseDownGridData = _mouseDownGridData;
            if (mouseDownGridData?.item is null) return;

            var iClickable = mouseDownGridData.GetComponent<IClickable>();
            if (iClickable is null)
            {
                BoardExtension.Selector.VisibilityRequest.Invoke(false);
                return;
            }

            var cameraPosition = _mainCamera.GetCameraPosition();
            var mouseUpGridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);

            if (mouseUpGridData is null)
            {
                BoardExtension.Selector.VisibilityRequest.Invoke(true);
                HandleMouseUpOnEmptyGrid(mouseDownGridData);
                return;
            }

            var isSameGrid = mouseUpGridData.Coordinate == mouseDownGridData.Coordinate;
            if (isSameGrid)
            {
                BoardExtension.Selector.SetPositionRequest.Invoke(mouseDownGridData.Coordinate);
                BoardExtension.Selector.VisibilityRequest.Invoke(true);
                BoardExtension.Selector.ScaleUpDownRequest.Invoke();

                if (!_isDragActive && _isDoubleClick)
                    HandleGenerateItem(mouseDownGridData);
                else
                    HandleMouseUpOnSameGrid(mouseDownGridData);
                return;
            }

            if (ShouldSwapItems(mouseDownGridData, mouseUpGridData))
                HandleItemSwap(mouseDownGridData, mouseUpGridData);
            else
                HandleItemMerge(mouseDownGridData, mouseUpGridData);

            BoardExtension.Selector.SetPositionRequest.Invoke(mouseUpGridData.Coordinate);
            BoardExtension.Selector.VisibilityRequest.Invoke(true);
        }

        private void HandleGenerateItem(GridData mouseDownGridData)
        {
            _boardGenerateController.TryGenerate(mouseDownGridData);

            mouseDownGridData.GetComponent<IClickable>()?.MouseUp();

            _isDragActive = false;
            _draggable = null;
        }

        private void HandleMouseUpOnEmptyGrid(GridData mouseDownGridData)
        {
            _boardTransferController.TryTransfer(TransferAction.Move, mouseDownGridData);

            mouseDownGridData.GetComponent<IClickable>()?.MouseUp();

            _isDragActive = false;
            _draggable = null;
        }

        private void HandleMouseUpOnSameGrid(GridData mouseDownGridData)
        {
            _boardTransferController.TryTransfer(TransferAction.Move, mouseDownGridData);
            _boardScaleUpDownController.TryScaleUpDown(mouseDownGridData);

            mouseDownGridData.GetComponent<IClickable>()?.MouseUp();

            _isDragActive = false;
            _draggable = null;
        }

        private bool ShouldSwapItems(GridData mouseDownGridData, GridData mouseUpGridData)
        {
            var downItemData = mouseDownGridData.itemDataSo;
            var upItemData = mouseUpGridData.itemDataSo;

            var hasDifferentItemType = downItemData.itemType != upItemData.itemType;
            var hasDifferentSpecialId = downItemData.GetSpecialId() != upItemData.GetSpecialId();
            var hasDifferentLevel = downItemData.level != upItemData.level;
            var isMaxLevel = upItemData.nextItemDataSo == null;

            return hasDifferentItemType || hasDifferentSpecialId || hasDifferentLevel || isMaxLevel;
        }

        private void HandleItemSwap(GridData mouseDownGridData, GridData mouseUpGridData)
        {
            _boardTransferController.TryTransfer(TransferAction.Swap, mouseDownGridData, mouseUpGridData);
            _boardScaleUpDownController.TryScaleUpDown(mouseUpGridData);

            mouseUpGridData.GetComponent<IClickable>()?.MouseUp();

            _isDragActive = false;
            _draggable = null;
        }

        private void HandleItemMerge(GridData mouseDownGridData, GridData mouseUpGridData)
        {
            _boardMergeController.Merge(mouseDownGridData, mouseUpGridData);

            _isDragActive = false;
            _draggable = null;
        }

        #region Unity Action

        private void OnEnable()
        {
            InputExtension.MouseUpRequested += MouseUpRequested;
            InputExtension.MouseDragRequested += MouseDragRequested;
            InputExtension.MouseDownRequested += MouseDownRequested;
        }

        private void OnDisable()
        {
            InputExtension.MouseUpRequested -= MouseUpRequested;
            InputExtension.MouseDragRequested -= MouseDragRequested;
            InputExtension.MouseDownRequested -= MouseDownRequested;
        }

        #endregion

        #region Parameters

        private IDraggable _draggable;
        private bool _isDragActive;
        private Vector2 _firstClickedPosition;

        private GridData _mouseDownGridData;
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