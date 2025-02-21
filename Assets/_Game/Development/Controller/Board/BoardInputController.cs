using System;
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
        
        private void OnMouseDownEvent(Vector2 vector2)
        {
            var cameraPosition = _mainCamera.GetCameraPosition();
            var gridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);
            if (gridData.item is null)
            {
                _scaleUpDown = null;
                _clickable = null;
                _firstClickedGridData = null;
                
                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
                return;
            }

            BoardExtension.Selector.RequestSetPosition.Invoke(gridData.coordinate);
            BoardExtension.Selector.RequestChangeVisibility.Invoke(true);

            if (_firstClickedGridData == gridData) _doubleClickGridData = gridData;

            _firstClickedPosition = cameraPosition;

            _scaleUpDown = gridData.GetComponent<IScaleUpDown>();
            _clickable = gridData.GetComponent<IClickable>();

            _clickable?.OnDown();
            _firstClickedGridData = gridData;
            
            Debug.Log("OnMouseDownEvent");
        }

        private void OnMouseDragEvent(Vector2 vector2)
        {
            var cameraPosition = _mainCamera.GetCameraPosition();

            if (_isDragging)
            {
                Debug.Log("_isDragging");
                _clickable?.OnDrag(cameraPosition);
                return;
            }

            var magnitude = (_firstClickedPosition - cameraPosition).magnitude;
            if (magnitude > _moveThreshold)
            {
                _isDragging = true;
                BoardExtension.Selector.RequestChangeVisibility.Invoke(false);
            }
        }

        private void OnMouseUpEvent(Vector2 vector2)
        {
            Debug.Log("OnMouseUpEvent");

            HandleClickUp().Forget();
        }
 
        private async UniTask HandleClickUp()
        {
            _isDragging = false;
            
            var cameraPosition = _mainCamera.GetCameraPosition();
            var currentGridData = BoardExtension.GetGridDataByCoordinate(cameraPosition);

            var canMerge = await _boardTransferController.TryTransfer(currentGridData, _firstClickedGridData);
            if (canMerge)
            {
                _boardMergeController.TryMerge(currentGridData,_firstClickedGridData);

                currentGridData.GetComponent<IClickable>().OnUp();
                currentGridData.GetComponent<IScaleUpDown>().ScaleUpDownAsync(_scaleUpDownDataSo).Forget();

                BoardExtension.Selector.RequestSetPosition.Invoke(currentGridData.coordinate);
                BoardExtension.Selector.RequestChangeVisibility.Invoke(true);
            }
            else
            {
                if (_firstClickedGridData is not null)
                {
                    BoardExtension.Selector.RequestSetPosition.Invoke(currentGridData.coordinate);
                    BoardExtension.Selector.RequestChangeVisibility.Invoke(true);
                    await BoardExtension.Selector.RequestScaleUpDown.Invoke();
                }
                
                _clickable?.OnUp();
                _scaleUpDown?.ScaleUpDownAsync(_scaleUpDownDataSo).Forget();
            }
        }

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