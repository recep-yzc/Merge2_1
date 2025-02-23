using _Game.Development.Interface.Ability;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardScaleUpDownController : MonoBehaviour
    {
        #region Parameters

        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        #endregion

        public void TryScaleUpDown(GridData gridData)
        {
            if (gridData?.item is null) return;
            var scaleUpDown = gridData.GetComponent<IScaleUpDown>();

            scaleUpDown?.ScaleUpDownAsync(_scaleUpDownDataSo).Forget();
        }
    }
}