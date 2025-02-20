using _Game.Development.Enum.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Factory.Item
{
    public class EmptyFactory : ItemFactory
    {
        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.None;

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            return null;
        }

        protected override ItemSaveData CreateItemSaveData(SerializableVector2 coordinate, ItemDataSo itemDataSo)
        {
            var dataSo = (EmptyItemDataSo)itemDataSo;
            return new EmptyItemSaveData(coordinate, dataSo.level, dataSo.itemType.ToInt(), 0);
        }
    }
}