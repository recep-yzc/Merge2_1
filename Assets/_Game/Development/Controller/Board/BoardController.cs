using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardController : MonoBehaviour
    {
        #region Parameters

        [Inject] private BoardLoadController _boardLoadController;

        #endregion

        private void Start()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;

            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            _boardLoadController.InitBoardJsonData();
            await _boardLoadController.CreateBoard(); // board oluşturulmasını bekler 
            // sahne yüklenebilir
        }
    }
}