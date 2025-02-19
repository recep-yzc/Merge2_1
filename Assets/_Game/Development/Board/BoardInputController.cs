using _Game.Development.Item;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Board
{
    public class BoardInputController : MonoBehaviour
    {
        #region Parameters

        private Camera _mainCamera;

        #endregion

        private async UniTask HandleTileClick(Vector3 inputPosition)
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(inputPosition);
            if (gridData is null) return;

            if (gridData.GameObject.TryGetComponent<IClickable>(out var clickable)) clickable.OnClick();

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
            if (!Input.GetMouseButtonDown(0)) return;

            var inputPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            HandleTileClick(inputPosition).Forget();
        }

        #endregion
    }
}