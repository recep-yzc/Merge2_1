using System;
using System.Collections.Generic;
using System.Globalization;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using _Game.Development.Item.Serializable;
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

            CreateItemSaveDataBySpecialId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemBySpecialId.TryAdd(ItemType.ToInt(), CreateItem);
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

                dictionary[itemDataSo.level] = itemDataSo;
            }
        }

        public override GameObject CreateItem(GridData gridData)
        {
            var item = GetOrCreateItemInPool(ref CreatedItemList, generatorPrefab.gameObject);
            var iGenerator = item.GetComponent<IGenerator>();

            var itemDataSo = (GeneratorItemDataSo)gridData.ItemDataSo;
            var dataSo = _itemDataListByGeneratorType[itemDataSo.generatorType.ToInt()][itemDataSo.level];

            iGenerator.SetParent(transform);
            iGenerator.SetPosition(gridData.Coordinate);
            iGenerator.SetSprite(dataSo.icon);

            return item;
        }

        public override ItemSaveData CreateItemSaveData(GridInspectorData gridInspectorData)
        {
            var generatorItemDataSo = (GeneratorItemDataSo)gridInspectorData.itemDataSo;
            return new GeneratorItemSaveData(gridInspectorData.coordinate, generatorItemDataSo.level,
                generatorItemDataSo.itemType.ToInt(), generatorItemDataSo.generatorType.ToInt(), MinDateTimeStr);
        }

        #region Parameters

        private readonly Dictionary<int, Dictionary<int, GeneratorItemDataSo>> _itemDataListByGeneratorType = new();
        private static readonly string MinDateTimeStr = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);

        #endregion
    }
}