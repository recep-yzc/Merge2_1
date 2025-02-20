using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board.Controller
{
    public class BoardInputController : MonoBehaviour
    {
        private UniTaskVoid ClickDown(Vector3 coordinate)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(coordinate);
            if (gridData?.GameObject is null) return default;

            _clickable = gridData.GetComponent<IClickable>();
            _clickable?.OnDown();

            _clickedGridData = gridData;

            return default;
        }

        private async UniTaskVoid ClickUp(Vector3 coordinate)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(coordinate);
            var wasIsTransferred = await _boardTransferController.TryTransfer(gridData, _clickedGridData);

            _clickable?.OnUp(wasIsTransferred);
            _clickedGridData = null;
        }

        private void FetchCameraData()
        {
            _mainCamera = Camera.main;
        }

        #region Parameters

        private Camera _mainCamera;

        private GridData _clickedGridData;
        private IClickable _clickable;

        private Vector2 _firstClickedCoordinate;
        private readonly float _moveThreshold = 0.1f;

        [Inject] private BoardTransferController _boardTransferController;
        [Inject] private BoardMergeController _boardMergeController;

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

            if (Input.GetMouseButton(0))
            {
                var secondClickedCoordinate = _mainCamera.GetCoordinate();
                var magnitude = (_firstClickedCoordinate - secondClickedCoordinate).magnitude;

                if (magnitude > _moveThreshold)
                {
                    _clickable.OnDrag(secondClickedCoordinate);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                var secondClickedCoordinate = _mainCamera.GetCoordinate();
                ClickUp(secondClickedCoordinate).Forget();
            }
        }

        #endregion
    }
}