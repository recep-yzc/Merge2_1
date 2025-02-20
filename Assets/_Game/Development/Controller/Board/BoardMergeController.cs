using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardMergeController : MonoBehaviour
    {
        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        public bool TryMerge(GridData gridData, GridData clickedGridData)
        {
            return gridData is not null && Merge(gridData, clickedGridData);
        }

        private bool Merge(GridData gridData, GridData clickedGridData)
        {
            var nexItemDataSo = gridData.itemDataSo.nextItemDataSo;

            gridData.itemDataSo = nexItemDataSo;
            var iItem = gridData.GetComponent<IItem>();
            iItem.SetSprite(nexItemDataSo.icon);
            iItem.ScaleUpDown();

            clickedGridData.itemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            clickedGridData.GetComponent<IItem>().PlayBackPool();
            clickedGridData.gameObject = null;

            return true;
        }
    }
}