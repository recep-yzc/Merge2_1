using System.Collections.Generic;
using _Game.Development.Extension;
using _Game.Development.Grid;
using _Game.Development.Level;
using UnityEngine;

namespace _Game.Development.Item
{
    public class ProductFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Product productPrefab;

        [SerializeField] private List<ProductItemDataSo> itemDataSoList;

        #region Parameters

        private readonly Dictionary<int, Dictionary<int, ProductItemDataSo>> _itemDataListByProductType = new();

        #endregion

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Product;
            FetchItemDataList();

            CreateItemSaveDataByCategoryType.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByCategoryType.TryAdd(ItemType.ToInt(), CreateItem);
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

                dictionary[itemDataSo.uniqueId] = itemDataSo;
            }
        }

        public override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(ref CreatedItemList, productPrefab.gameObject);
            var iProduct = item.GetComponent<IProduct>();

            var dataSo = _itemDataListByProductType[itemSaveData.categoryType][itemSaveData.uniqueId];

            iProduct.SetParent(transform);
            iProduct.SetPosition(itemSaveData.coordinate);
            iProduct.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(LevelGridData levelGridData)
        {
            var dataSo = (ProductItemDataSo)levelGridData.itemDataSo;
            return new ProductItemSaveData(levelGridData.coordinate, dataSo.uniqueId, dataSo.itemType.ToInt(),
                dataSo.productType.ToInt());
        }
    }
}