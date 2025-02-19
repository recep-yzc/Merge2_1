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

        public override GameObject CreateItem(GridData gridData)
        {
            var item = GetOrCreateItemInPool(ref CreatedItemList, generatorPrefab.gameObject);
            var iGenerator = item.GetComponent<IGenerator>();

            var itemDataSo = (GeneratorItemDataSo)gridData.itemDataSo;
            var dataSo = _itemDataListByGeneratorType[itemDataSo.generatorType.ToInt()][itemDataSo.uniqueId];

            iGenerator.SetParent(transform);
            iGenerator.SetPosition(gridData.coordinate);
            iGenerator.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(GridData gridData)
        {
            var generatorItemDataSo = (GeneratorItemDataSo)gridData.itemDataSo;
            return new GeneratorItemSaveData(gridData.coordinate, generatorItemDataSo.uniqueId,
                generatorItemDataSo.itemType.ToInt(), generatorItemDataSo.generatorType.ToInt(),
                generatorItemDataSo.spawnAmount, MinDateTimeStr);
        }

        #region Parameters

        private readonly Dictionary<int, Dictionary<int, GeneratorItemDataSo>> _itemDataListByGeneratorType = new();
        private static readonly string MinDateTimeStr = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);

        #endregion
    }
}