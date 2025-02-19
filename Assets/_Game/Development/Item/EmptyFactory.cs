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

        public override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            return null;
        }

        public override ItemSaveData CreateItemSaveData(LevelGridData levelGridData)
        {
            var dataSo = (EmptyItemDataSo)levelGridData.itemDataSo;
            return new EmptyItemSaveData(levelGridData.coordinate, dataSo.uniqueId, dataSo.itemType.ToInt(), 0);
        }
    }
}