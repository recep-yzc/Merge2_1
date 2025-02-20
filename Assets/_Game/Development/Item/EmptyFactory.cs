using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using UnityEngine;

namespace _Game.Development.Item
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