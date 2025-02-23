using _Game.Development.Enum.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Object.Item;
using _Game.Development.Scriptable.Factory;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Factory.Item
{
    public class ProductFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Product productPrefab;

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;

        #endregion

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Product;

            CreateDefaultItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateDefaultItemSaveData);
            CreateEditedItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateEditedItemSaveData);

            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        private GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(productPrefab.gameObject);

            var iItem = item.GetComponent<IItem>();
            iItem.ResetParameters();

            var iProduct = item.GetComponent<IProduct>();
            var iPool = item.GetComponent<IPool>();

            void DespawnAction()
            {
                Despawn(item);
            }

            iPool.RegisterDespawnCallback(DespawnAction);
            var itemDataSo = _allItemDataSo.GetItemDataSoByItemSaveData(itemSaveData);

            iProduct.SetParent(transform);
            iProduct.SetPosition(itemSaveData.coordinate.ToVector2());
            iProduct.SetSprite(itemDataSo.icon);

            iProduct.SetItemDataSo(itemDataSo);
            iProduct.FetchItemData();

            return item;
        }

        private ItemSaveData CreateDefaultItemSaveData(DefaultSaveParameters defaultSaveParameters)
        {
            var editedSave = new EditedSaveParameters(defaultSaveParameters.coordinate, defaultSaveParameters.itemDataSo);
            return CreateItemSaveData(editedSave);
        }

        private ItemSaveData CreateEditedItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            return CreateItemSaveData(editedSaveParameters);
        }

        private ItemSaveData CreateItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            if (editedSaveParameters.itemDataSo is not ProductItemDataSo productItemDataSo) return default;

            var jsonCoordinate = editedSaveParameters.coordinate.ToJsonVector2();
            var level = productItemDataSo.level;
            var itemId = productItemDataSo.GetItemId();
            var specialId = productItemDataSo.GetSpecialId();

            return new ProductItemSaveData(jsonCoordinate, level, itemId, specialId);
        }
    }
}