using System;
using System.Collections.Generic;
using System.Globalization;
using _Game.Development.Extension;
using _Game.Development.Grid;
using _Game.Development.Level;
using UnityEngine;

namespace _Game.Development.Item
{
    public class GeneratorFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Generator generatorPrefab;

        [SerializeField] private List<GeneratorItemDataSo> itemDataSoList;

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Generator;
            FetchItemDataList();

            CreateItemSaveDataByCategoryType.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByCategoryType.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        private void FetchItemDataList()
        {
            foreach (var itemDataSo in itemDataSoList)
            {
                var type = itemDataSo.generatorType.ToInt();
                if (!_itemDataListByGeneratorType.TryGetValue(type, out var dictionary))
                {
                    dictionary = new Dictionary<int, GeneratorItemDataSo>();
                    _itemDataListByGeneratorType[type] = dictionary;
                }

                dictionary[itemDataSo.uniqueId] = itemDataSo;
            }
        }

        public override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(ref CreatedItemList, generatorPrefab.gameObject);
            var iGenerator = item.GetComponent<IGenerator>();

            var dataSo = _itemDataListByGeneratorType[itemSaveData.categoryType][itemSaveData.uniqueId];

            iGenerator.SetParent(transform);
            iGenerator.SetPosition(itemSaveData.coordinate);
            iGenerator.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(LevelGridData levelGridData)
        {
            var generatorItemDataSo = (GeneratorItemDataSo)levelGridData.itemDataSo;
            return new GeneratorItemSaveData(levelGridData.coordinate, generatorItemDataSo.uniqueId,
                generatorItemDataSo.itemType.ToInt(), generatorItemDataSo.generatorType.ToInt(),
                generatorItemDataSo.spawnAmount, MinDateTimeStr);
        }

        #region Parameters

        private readonly Dictionary<int, Dictionary<int, GeneratorItemDataSo>> _itemDataListByGeneratorType = new();
        private static readonly string MinDateTimeStr = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);

        #endregion
    }
}