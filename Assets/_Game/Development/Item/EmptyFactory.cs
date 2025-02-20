using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using _Game.Development.Item.Serializable;
using UnityEngine;

namespace _Game.Development.Item
{
    public class EmptyFactory : ItemFactory
    {
        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.None;

            CreateItemSaveDataBySpecialId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemBySpecialId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        public override GameObject CreateItem(GridData gridData)
        {
            return null;
        }

        public override ItemSaveData CreateItemSaveData(GridInspectorData gridInspectorData)
        {
            var dataSo = (EmptyItemDataSo)gridInspectorData.itemDataSo;
            return new EmptyItemSaveData(gridInspectorData.coordinate, dataSo.level, dataSo.itemType.ToInt(), 0);
        }
    }
}