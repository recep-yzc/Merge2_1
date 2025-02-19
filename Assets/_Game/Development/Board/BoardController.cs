using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board
{
    public class BoardController : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            var boardSaveData = _boardLoadController.Load();
            await _boardCreateController.Create(boardSaveData);
            //_boardSpawnController.CreateGridData();
            //_boardSpawnController.FetchGridNeighbor();

            //_boardViewController.TryUpdateView().Forget();
        }

        #region Parameters

        [Inject] private BoardLoadController _boardLoadController;

        [Inject] private BoardCreateController _boardCreateController;
        //[Inject] private BoardViewController _boardViewController;
        //[Inject] private BoardInputController _boardInputController;
        //[Inject] private BoardBlastController _boardBlastController;
        //[Inject] private BoardFallController _boardFallController;

        //[Inject] private BoardScaleUpDownController _boardScaleUpDownController;
        //[Inject] private BoardShakeController _boardShakeController;

        #endregion
    }
}