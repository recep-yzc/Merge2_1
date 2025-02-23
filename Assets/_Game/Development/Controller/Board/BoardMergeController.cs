using _Game.Development.Enum.Item;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardMergeController : MonoBehaviour
    {
        public void Merge(GridData mouseDownGridData, GridData mouseUpGridData)
        {
            UpgradeItem(mouseUpGridData);
            CreateEmptyItem(mouseDownGridData);
        }

        private void CreateEmptyItem(GridData gridData)
        {
            gridData.GetComponent<IPool>().PlayDespawnPool();
            gridData.itemDataSo = _allItemDataSo.GetEmptyItemDataSo();

            var itemId = ItemType.Empty.ToInt();
            var itemSaveData = ItemFactory.CreateItemSaveDataByItemId[itemId]
                .Invoke(gridData.coordinate, gridData.itemDataSo);
            gridData.item = ItemFactory.CreateItemByItemId[itemId].Invoke(itemSaveData);
        }

        private void UpgradeItem(GridData gridData)
        {
            gridData.GetComponent<IScaleUpDown>().ScaleUpDownAsync(_scaleUpDownDataSo).Forget();
            gridData.itemDataSo = gridData.itemDataSo.nextItemDataSo;

            var iItem = gridData.GetComponent<IItem>();
            iItem.SetItemDataSo(gridData.itemDataSo);
            iItem.SetSprite(gridData.itemDataSo.icon);
            iItem.FetchItemData();
            iItem.LevelUp();
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        #endregion
    }
}