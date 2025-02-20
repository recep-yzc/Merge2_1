using _Game.Development.Grid.Serializable;
using _Game.Development.Item;
using _Game.Development.Item.Scriptable;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board.Controller
{
    public class BoardMergeController : MonoBehaviour
    {
        [Inject] AllItemDataSo _allItemDataSo;

        public async UniTask<bool> TryMerge(GridData gridData, GridData clickedGridData)
        {
            if (gridData is null)
            {
                return false;
            }

            return await Merge(gridData, clickedGridData);
        }

        async UniTask<bool> Merge(GridData gridData, GridData clickedGridData)
        {
            var nexItemDataSo = gridData.ItemDataSo.nextItemDataSo;
            
            gridData.ItemDataSo = nexItemDataSo;
            gridData.GetComponent<IItem>().SetSprite(nexItemDataSo.icon);
            
            clickedGridData.ItemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            clickedGridData.GetComponent<IItem>().PlayBackPool();
            clickedGridData.GameObject = null;

            return true;
        }
    }
}