using _Game.Development.Extension;
using _Game.Development.Grid;
using _Game.Development.Level;
using UnityEngine;

namespace _Game.Development.Item
{
    public class EmptyFactory : ItemFactory
    {
        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.None;

            CreateItemSaveDataByCategoryType.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByCategoryType.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        public override GameObject CreateItem(GridData gridData)
        {
            return null;
        }

        public override ItemSaveData CreateItemSaveData(GridData gridData)
        {
            var dataSo = (EmptyItemDataSo)gridData.itemDataSo;
            return new EmptyItemSaveData(gridData.coordinate, dataSo.level, dataSo.itemType.ToInt(), 0);
        }
    }
}