using _Game.Development.Factory.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Factory;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardMergeController : MonoBehaviour
    {
        public void Merge(GridData sourceGrid, GridData targetGrid)
        {
            _boardScaleUpDownController.TryScaleUpDown(targetGrid);

            UpgradeItem(targetGrid);
            DespawnItem(sourceGrid);
            CreateEmptyItem(sourceGrid);
        }

        private void DespawnItem(GridData gridData)
        {
            gridData.GetComponent<IPool>().InvokeDespawn();
        }

        private void CreateEmptyItem(GridData gridData)
        {
            var itemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            var itemId = itemDataSo.GetItemId();
            var coordinate = gridData.Coordinate;

            var itemSaveData = GenerateItemSaveData(itemId, coordinate, itemDataSo);
            var item = InstantiateItem(itemId, itemSaveData);

            gridData.item = item;
            gridData.itemDataSo = itemDataSo;
        }

        private ItemSaveData GenerateItemSaveData(int itemId, Vector2 coordinate, ItemDataSo itemDataSo)
        {
            var defaultParameters = new DefaultSaveParameters(coordinate, itemDataSo);
            return ItemFactory.CreateDefaultItemSaveDataByItemId[itemId].Invoke(defaultParameters);
        }

        private GameObject InstantiateItem(int itemId, ItemSaveData itemSaveData)
        {
            return ItemFactory.CreateItemByItemId[itemId].Invoke(itemSaveData);
        }

        private void UpgradeItem(GridData gridData)
        {
            gridData.itemDataSo = GetNextItemData(gridData.itemDataSo);
            ApplyItemUpgrade(gridData);
        }

        private ItemDataSo GetNextItemData(ItemDataSo itemDataSo)
        {
            return itemDataSo.nextItemDataSo;
        }

        private void ApplyItemUpgrade(GridData gridData)
        {
            var item = gridData.GetComponent<IItem>();
            item.SetItemDataSo(gridData.itemDataSo);
            item.SetSprite(gridData.itemDataSo.icon);
            item.FetchItemData();
            item.LevelUp();
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        [Inject] private BoardScaleUpDownController _boardScaleUpDownController;

        #endregion
    }
}