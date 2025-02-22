using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardMergeController : MonoBehaviour
    {
        public void TryMerge(GridData mouseDownGridData, GridData mouseUpGridData)
        {
            var nexItemDataSo = mouseUpGridData.itemDataSo.nextItemDataSo;

            mouseUpGridData.itemDataSo = nexItemDataSo;
            mouseUpGridData.GetComponent<IScaleUpDown>().ScaleUpDownAsync(_scaleUpDownDataSo).Forget();

            var iItem = mouseUpGridData.GetComponent<IItem>();
            iItem.SetSprite(nexItemDataSo.icon);
            iItem.SetItemDataSo(mouseUpGridData.itemDataSo);
            iItem.FetchItemData();

            mouseDownGridData.itemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            mouseDownGridData.GetComponent<IPool>().PlayDespawnPool();

            var itemSaveData = ItemFactory.CreateItemSaveDataByItemId[0]
                .Invoke(mouseDownGridData.coordinate, mouseDownGridData.itemDataSo);
            mouseDownGridData.item = ItemFactory.CreateItemByItemId[0].Invoke(itemSaveData);
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        #endregion
    }
}