using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board.Controller
{
    public class BoardController : MonoBehaviour
    {
        #region Parameters

        [Inject] private BoardLoadController _boardLoadController;

        //[Inject] private BoardViewController _boardViewController;
        //[Inject] private BoardInputController _boardInputController;
        //[Inject] private BoardBlastController _boardBlastController;
        //[Inject] private BoardFallController _boardFallController;

        //[Inject] private BoardScaleUpDownController _boardScaleUpDownController;
        //[Inject] private BoardShakeController _boardShakeController;

        #endregion

        private void Start()
        {
            Application.targetFrameRate = 60;
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            _boardLoadController.FetchLevelData();
            await _boardLoadController.Create();

            //_boardSpawnController.CreateGridData();
            //_boardSpawnController.FetchGridNeighbor();
            //_boardViewController.TryUpdateView().Forget();
        }
    }
}