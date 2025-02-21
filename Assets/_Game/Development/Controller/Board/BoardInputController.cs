using _Game.Development.Abstract.Board;
using _Game.Development.Enum.Board;
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
        #region Unity Action

        private void Update()
        {
            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
                return;

            switch (GetMouseAction())
            {
                case MouseAction.Down:
                    HandleClickDown().Forget();
                    break;
                case MouseAction.Hold:
                    HandleDrag();
                    break;
                case MouseAction.Up:
                    HandleClickUp().Forget();
                    break;
            }
        }

        #endregion

        private UniTaskVoid HandleClickDown()
        {
            var cameraPosition = _mainCamera.GetCameraPosition();
            var gridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);
            if (gridData.item is null)
            {
                BoardActions.Selector.RequestChangeVisibility.Invoke(false);
                return default;
            }

            BoardActions.Selector.RequestSetPosition.Invoke(gridData.coordinate);
            BoardActions.Selector.RequestChangeVisibility.Invoke(true);

            if (_firstClickedGridData == gridData) _doubleClickGridData = gridData;

            _firstClickedPosition = cameraPosition;

            _scaleUpDown = gridData.GetComponent<IScaleUpDown>();
            _clickable = gridData.GetComponent<IClickable>();

            _clickable?.OnDown();
            //_doubleClickGridData?.OnDoubleDown();

            _firstClickedGridData = gridData;

            return default;
        }

        private void HandleDrag()
        {
            var cameraPosition = _mainCamera.GetCameraPosition();

            if (_isDragging)
            {
                _clickable?.OnDrag(cameraPosition);
                return;
            }

            var magnitude = (_firstClickedPosition - cameraPosition).magnitude;
            if (magnitude > _moveThreshold)
            {
                _isDragging = true;
                BoardActions.Selector.RequestChangeVisibility.Invoke(false);
            }
        }

        private async UniTaskVoid HandleClickUp()
        {
            var cameraPosition = _mainCamera.GetCameraPosition();
            var currentGridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);

            var canMerge = await _boardTransferController.TryTransfer(currentGridData, _firstClickedGridData);
            if (canMerge)
            {
                _boardMergeController.TryMerge(currentGridData, _firstClickedGridData);

                currentGridData.GetComponent<IClickable>().OnUp();
                currentGridData.GetComponent<IScaleUpDown>().ScaleUpDownAsync(_scaleUpDownDataSo).Forget();

                BoardActions.Selector.RequestSetPosition.Invoke(currentGridData.coordinate);
                BoardActions.Selector.RequestChangeVisibility.Invoke(true);
            }
            else
            {
                BoardActions.Selector.RequestSetPosition.Invoke(currentGridData.coordinate);
                BoardActions.Selector.RequestChangeVisibility.Invoke(true);
                await BoardActions.Selector.RequestScaleUpDown.Invoke();

                _clickable?.OnUp();
                _scaleUpDown?.ScaleUpDownAsync(_scaleUpDownDataSo).Forget();
            }


            _clickable = null;
            _scaleUpDown = null;
            _firstClickedGridData = null;
            _isDragging = false;
        }

        #region Getter Setter

        private MouseAction GetMouseAction()
        {
            if (Input.GetMouseButtonDown(0)) return MouseAction.Down;
            if (Input.GetMouseButton(0)) return MouseAction.Hold;
            if (Input.GetMouseButtonUp(0)) return MouseAction.Up;
            return MouseAction.None;
        }

        #endregion

        //SaveInBackground(BoardExtension.JsonPath, BoardExtension.BoardJsonData).Forget();

        /*public static async UniTask SaveInBackground<T>(string fileName, T data)
        {
            await UniTask.RunOnThreadPool(async () =>
            {
                await SaveAsync(fileName, data);
            });
        }

        private static async Task SaveAsync<T>(string fileName, T data)
        {
            await Task.Delay(1);
            for (int i = 0; i < 10000; i++)
            {
                Debug.Log(i);
            }
        }*/

        #region Parameters

        private GridData _firstClickedGridData;
        private IClickable _clickable;
        private IScaleUpDown _scaleUpDown;

        private Vector2 _firstClickedPosition;
        private readonly float _moveThreshold = 0.1f;

        [Inject] private Camera _mainCamera;

        [Inject] private BoardTransferController _boardTransferController;
        [Inject] private BoardMergeController _boardMergeController;
        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        private bool _isDragging;
        private GridData _doubleClickGridData;

        #endregion
    }
}