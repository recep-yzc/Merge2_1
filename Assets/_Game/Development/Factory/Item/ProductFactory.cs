using _Game.Development.Enum.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Object.Item;
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

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData<ProductItemSaveData>);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(productPrefab.gameObject);

            var iItem = item.GetComponent<IItem>();
            iItem.ResetParameters();

            var iProduct = item.GetComponent<IProduct>();
            var iPool = item.GetComponent<IPool>();

            void DespawnPoolAction()
            {
                Despawn(item);
            }

            iPool.AddDespawnPool(DespawnPoolAction);
            var itemDataSo =
                _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);

            iProduct.SetParent(transform);
            iProduct.SetPosition(itemSaveData.coordinate.ToVector2());
            iProduct.SetSprite(itemDataSo.icon);

            iProduct.SetItemDataSo(itemDataSo);
            iProduct.FetchItemData();

            return item;
        }

        protected override T CreateItemSaveData<T>(Vector2 coordinate, ItemDataSo itemDataSo)
        {
            var dataSo = (ProductItemDataSo)itemDataSo;
            return new ProductItemSaveData(coordinate.ToJsonVector2(), dataSo.level, dataSo.itemType.ToInt(),
                dataSo.productType.ToInt()) as T;
        }
    }
}