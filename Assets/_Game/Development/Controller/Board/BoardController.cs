using System.Globalization;
using _Game.Development.Static;
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

        private async void Start()
        {
            Application.targetFrameRate = 60;
            Input.multiTouchEnabled = false;
            CultureInfo.CurrentCulture = CultureExtension.Parameters.CurrentCultureInfo;

            await Initialize();

            // sahne yüklenebilir
        }

        private async UniTask Initialize()
        {
            await _boardLoadController.InitializeBoardJsonData();
            await _boardLoadController.InitializeBoard(); // board oluşturulmasını bekler 
            BoardExtension.Parameters.IsBoardInitialized = true;
        }
    }
}