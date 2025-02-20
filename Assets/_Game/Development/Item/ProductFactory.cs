using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using UnityEngine;
using Zenject;

namespace _Game.Development.Item
{
    public class ProductFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Product productPrefab;

        [SerializeField] private List<ProductItemDataSo> itemDataSoList;

        #region Parameters

        [Inject] AllItemDataSo _allItemDataSo;
        private readonly Dictionary<int, Dictionary<int, ProductItemDataSo>> _itemDataListByProductType = new();

        #endregion

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Product;
            FetchItemDataList();

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        private void FetchItemDataList()
        {
            foreach (var itemDataSo in itemDataSoList)
            {
                var type = itemDataSo.productType.ToInt();
                if (!_itemDataListByProductType.TryGetValue(type, out var dictionary))
                {
                    dictionary = new Dictionary<int, ProductItemDataSo>();
                    _itemDataListByProductType[type] = dictionary;
                }

                dictionary[itemDataSo.level] = itemDataSo;
            }
        }

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(productPrefab.gameObject);
            var iProduct = item.GetComponent<IProduct>();
            
            iProduct.AddBackPool(() => BackPool(item));

            var productItemDataSo = _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);
            iProduct.SetParent(transform);
            iProduct.SetPosition(itemSaveData.coordinate.ToVector2());
            iProduct.SetSprite(productItemDataSo.icon);

            return item;
        }

        protected override ItemSaveData CreateItemSaveData(SerializableVector2 coordinate, ItemDataSo itemDataSo)
        {
            var dataSo = (ProductItemDataSo)itemDataSo;
            return new ProductItemSaveData(coordinate, dataSo.level, dataSo.itemType.ToInt(),
                dataSo.productType.ToInt());
        }
    }
}