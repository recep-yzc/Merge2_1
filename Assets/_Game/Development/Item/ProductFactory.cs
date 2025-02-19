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

                dictionary[itemDataSo.level] = itemDataSo;
            }
        }

        public override GameObject CreateItem(GridData gridData)
        {
            var item = GetOrCreateItemInPool(ref CreatedItemList, productPrefab.gameObject);
            var iProduct = item.GetComponent<IProduct>();

            var itemDataSo = (ProductItemDataSo)gridData.itemDataSo;
            var dataSo = _itemDataListByProductType[itemDataSo.productType.ToInt()][itemDataSo.level];

            iProduct.SetParent(transform);
            iProduct.SetPosition(gridData.coordinate);
            iProduct.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(GridData gridData)
        {
            var dataSo = (ProductItemDataSo)gridData.itemDataSo;
            return new ProductItemSaveData(gridData.coordinate, dataSo.level, dataSo.itemType.ToInt(),
                dataSo.productType.ToInt());
        }
    }
}