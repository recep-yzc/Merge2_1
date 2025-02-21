using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardMergeController : MonoBehaviour
    {
        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        public void TryMerge(GridData gridData, GridData clickedGridData)
        {
            var nexItemDataSo = gridData.itemDataSo.nextItemDataSo;

            gridData.itemDataSo = nexItemDataSo;
            var iItem = gridData.GetComponent<IItem>();
            iItem.SetSprite(nexItemDataSo.icon);

            clickedGridData.itemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            clickedGridData.GetComponent<IPool>().PlayDespawnPool();
            clickedGridData.item = null;
        }
    }
}