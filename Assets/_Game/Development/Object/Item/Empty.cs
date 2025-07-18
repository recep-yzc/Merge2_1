﻿using _Game.Development.Factory.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Factory;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;

namespace _Game.Development.Object.Item
{
    public class Empty : Item, IEmpty
    {
        #region Parameters

        private EmptyItemDataSo _emptyItemDataSo;

        #endregion

        #region Item

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            _emptyItemDataSo = (EmptyItemDataSo)itemDataSo;
        }

        public override ItemSaveData CreateEditedItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(SelfCoordinate);

            var coordinate = gridData.Coordinate;
            var itemDataSo = gridData.itemDataSo;
            var itemId = itemDataSo.GetItemId();

            var createEditedItemSaveDataFunc = ItemFactory.CreateEditedItemSaveDataByItemId[itemId];
            var editedItemSaveParameters = new EditedSaveParameters(coordinate, itemDataSo);
            return createEditedItemSaveDataFunc.Invoke(editedItemSaveParameters);
        }

        #endregion
    }
}