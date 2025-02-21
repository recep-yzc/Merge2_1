using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardInputController : MonoBehaviour
    {
        private UniTaskVoid ClickDown(Vector3 coordinate)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(coordinate);
            if (gridData?.gameObject is null) return default;

            _scaleUpDown = gridData.GetComponent<IScaleUpDown>();
            _clickable = gridData.GetComponent<IClickable>();
            _clickable?.OnDown();

            _clickedGridData = gridData;

            return default;
        }

        private async UniTaskVoid ClickUp(Vector3 coordinate)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(coordinate);
            var wasMerged = await _boardTransferController.TryTransfer(gridData, _clickedGridData);

            if (wasMerged)
            {
                gridData.GetComponent<IScaleUpDown>().ScaleUpDownAsync(_scaleUpDownDataSo);
            }

            _clickable?.OnUp();
            _scaleUpDown?.ScaleUpDownAsync(_scaleUpDownDataSo);

            _clickable = null;
            _scaleUpDown = null;
            _clickedGridData = null;
        }

        private void FetchCameraData()
        {
            _mainCamera = Camera.main;
        }

        private void Drag(Vector2 secondClickedCoordinate)
        {
            var magnitude = (_firstClickedCoordinate - secondClickedCoordinate).magnitude;
            if (magnitude > _moveThreshold) _clickable?.OnDrag(secondClickedCoordinate);
        }

        #region Parameters

        private Camera _mainCamera;

        private GridData _clickedGridData;
        private IClickable _clickable;
        private IScaleUpDown _scaleUpDown;

        private Vector2 _firstClickedCoordinate;
        private readonly float _moveThreshold = 0.1f;

        [Inject] private BoardTransferController _boardTransferController;
        [Inject] private BoardMergeController _boardMergeController;

        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        #endregion

        #region Unity Action

        private void Start()
        {
            FetchCameraData();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstClickedCoordinate = _mainCamera.GetCoordinate();
                ClickDown(_firstClickedCoordinate).Forget();
            }
            else if (Input.GetMouseButton(0))
            {
                var secondClickedCoordinate = _mainCamera.GetCoordinate();
                Drag(secondClickedCoordinate);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                var secondClickedCoordinate = _mainCamera.GetCoordinate();
                ClickUp(secondClickedCoordinate).Forget();
            }
        }

        #endregion
    }
}