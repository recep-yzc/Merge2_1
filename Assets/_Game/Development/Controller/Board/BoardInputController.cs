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
        private void OnMouseDownEvent(Vector2 vector2)
        {
            _firstClickedPosition = _mainCamera.GetCameraPosition();
            var gridData = BoardExtension.GetGridDataByCoordinate(_firstClickedPosition);

            if (gridData?.item is null)
            {
                _mouseDownGridData = null;
                _isDoubleClick = false;

                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
                return;
            }

            _mouseDownGridData = gridData;

            var iClickable = gridData.GetComponent<IClickable>();
            if (iClickable is null)
            {
                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
                return;
            }

            _isDoubleClick = _mouseDownGridData == gridData;

            BoardExtension.Selector.RequestSetPosition.Invoke(gridData.coordinate);
            BoardExtension.Selector.RequestChangeVisibility.Invoke(true);

            iClickable.MouseDown();
            _draggable = gridData.GetComponent<IDraggable>();
        }

        private void OnMouseDragEvent(Vector2 vector2)
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
                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
            }
        }

        private void OnMouseUpEvent(Vector2 vector2)
        {
            var mouseDownGridData = _mouseDownGridData;
            if (mouseDownGridData?.item is null) return;
            
            var iClickable = mouseDownGridData.GetComponent<IClickable>();
            if (iClickable is null)
            {
                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
                return;
            }
            
            var cameraPosition = _mainCamera.GetCameraPosition();
            var mouseUpGridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);

            if (mouseUpGridData is null)
            {
                BoardExtension.Selector.RequestChangeVisibility.Invoke(true);
                HandleMouseUpOnEmptyGrid(mouseDownGridData);
                return;
            }

            var isSameGrid = mouseUpGridData.coordinate == mouseDownGridData.coordinate;
            if (isSameGrid)
            {
                BoardExtension.Selector.RequestSetPosition.Invoke(mouseDownGridData.coordinate);
                BoardExtension.Selector.RequestChangeVisibility.Invoke(true);
                BoardExtension.Selector.RequestScaleUpDown.Invoke();

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

            BoardExtension.Selector.RequestSetPosition.Invoke(mouseUpGridData.coordinate);
            BoardExtension.Selector.RequestChangeVisibility.Invoke(true);
        }

        private void HandleGenerateItem(GridData mouseDownGridData)
        {
            _boardGenerateController.TryGenerate(mouseDownGridData);

            _isDragActive = false;
            _draggable = null;
        }

        private void HandleMouseUpOnEmptyGrid(GridData mouseDownGridData)
        {
            _boardTransferController.TryTransfer(TransferAction.Move, mouseDownGridData);

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
            InputExtension.OnMouseUpEvent += OnMouseUpEvent;
            InputExtension.OnMouseDragEvent += OnMouseDragEvent;
            InputExtension.OnMouseDownEvent += OnMouseDownEvent;
        }

        private void OnDisable()
        {
            InputExtension.OnMouseUpEvent -= OnMouseUpEvent;
            InputExtension.OnMouseDragEvent -= OnMouseDragEvent;
            InputExtension.OnMouseDownEvent -= OnMouseDownEvent;
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