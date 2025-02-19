using _Game.Development.Item;
using _Game.Development.Level;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game.Development.Board
{
    public class BoardInputController : MonoBehaviour
    {
        #region Parameters

        private Camera _mainCamera;
        private GridData _clickedGridData;
        private Vector2 _firstPosition;

        #endregion

        private UniTaskVoid HandleGridClickDown(Vector3 inputPosition)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(inputPosition);
            if (gridData?.GameObject is null) return default;

            if (gridData.GameObject.TryGetComponent<IClickable>(out var clickable)) clickable.OnClick();
            _clickedGridData = gridData;

            /* var blastedTileDataList = await _boardBlastController.TryBlast(levelGridData);
             if (blastedTileDataList?.Count > 0)
             {
                 _boardFallController.TryFall().Forget();
                 _boardSpawnController.TryCreate().Forget();

                 _boardFallController.TryFall().Forget();
                 await _boardViewController.TryUpdateView();
                 return;
             }

             _boardShakeController.TryShake(levelGridData);
             _boardScaleUpDownController.TryScaleUpDown(levelGridData);*/

            return default;
        }

        private UniTaskVoid HandleGridClickUp(Vector3 inputPosition)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(inputPosition);
            if (gridData is null)
            {
                _clickedGridData.GameObject.transform.DOMove(_clickedGridData.coordinate, 0.3f);
                return default;
            }

            if (gridData.itemDataSo.itemType == _clickedGridData.itemDataSo.itemType && gridData.itemDataSo.level == _clickedGridData.itemDataSo.level)
            {
                if (gridData.coordinate == _clickedGridData.coordinate)
                {
                    _clickedGridData.GameObject.transform.DOMove(_clickedGridData.coordinate, 0.3f);
                }
                else
                {
                    
                }
            }
            else
            {
                if (gridData.GameObject is not null)
                {
                    SwapGridDataProperties(gridData, _clickedGridData);
                    
                    gridData.GameObject?.transform.DOMove(gridData.coordinate, 0.3f);
                    _clickedGridData.GameObject?.transform.DOMove(_clickedGridData.coordinate, 0.3f);
                }
                else
                {
                    SwapGridDataProperties(gridData, _clickedGridData);
                    _clickedGridData.GameObject?.transform.DOMove(gridData.coordinate, 0.3f);
                    gridData.GameObject?.transform.DOMove(gridData.coordinate, 0.3f);
                }
            }

            return default;
        }

        private void SwapGridDataProperties(GridData gridData, GridData newGridData)
        {
            (newGridData.GameObject, gridData.GameObject) = (gridData.GameObject, newGridData.GameObject);
            (newGridData.itemDataSo, gridData.itemDataSo) = (gridData.itemDataSo, newGridData.itemDataSo);
        }
        
        private void FetchCameraData()
        {
            _mainCamera = Camera.main;
        }

        #region Unity Action

        private void Start()
        {
            FetchCameraData();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _firstPosition=_mainCamera.ScreenToWorldPoint(Input.mousePosition);
                HandleGridClickDown(_firstPosition).Forget();
            }

            if (Input.GetMouseButton(0))
            {
                if (_clickedGridData is not null)
                {
                    Vector2 position=_mainCamera.ScreenToWorldPoint(Input.mousePosition);

                    if ((_firstPosition - position).magnitude > 0.1f)
                    {
                        _clickedGridData.GameObject.transform.position = position;
                    }
                }
            }
            
            if (Input.GetMouseButtonUp(0))
            {
                if (_clickedGridData is not null)
                {
                    var position=_mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    position.z = 0;
                    HandleGridClickUp(position).Forget();
                    _clickedGridData = null;
                }
            }
        }

        #endregion
    }
}