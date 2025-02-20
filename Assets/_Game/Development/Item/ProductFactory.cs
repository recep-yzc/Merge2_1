using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using _Game.Development.Item.Serializable;
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

            CreateItemSaveDataBySpecialId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemBySpecialId.TryAdd(ItemType.ToInt(), CreateItem);
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

            var itemDataSo = (ProductItemDataSo)gridData.ItemDataSo;
            var dataSo = _itemDataListByProductType[itemDataSo.productType.ToInt()][itemDataSo.level];

            iProduct.SetParent(transform);
            iProduct.SetPosition(gridData.Coordinate);
            iProduct.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(GridInspectorData gridInspectorData)
        {
            var dataSo = (ProductItemDataSo)gridInspectorData.itemDataSo;
            return new ProductItemSaveData(gridInspectorData.coordinate, dataSo.level, dataSo.itemType.ToInt(),
                dataSo.productType.ToInt());
        }
    }
}