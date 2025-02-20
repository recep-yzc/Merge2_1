using System;
using System.Collections.Generic;
using System.Globalization;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using UnityEngine;
using Zenject;

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

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
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

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(generatorPrefab.gameObject);
            var iGenerator = item.GetComponent<IGenerator>();
            iGenerator.AddBackPool(() => BackPool(item));
            
            var generatorItemDataSo = _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);

            iGenerator.SetParent(transform);
            iGenerator.SetPosition(itemSaveData.coordinate.ToVector2());
            iGenerator.SetSprite(generatorItemDataSo.icon);

            return item;
        }

        protected override ItemSaveData CreateItemSaveData(SerializableVector2 coordinate, ItemDataSo itemDataSo)
        {
            var dataSo = (GeneratorItemDataSo)itemDataSo;
            return new GeneratorItemSaveData(coordinate, dataSo.level, dataSo.itemType.ToInt(),
                dataSo.generatorType.ToInt(), MinDateTimeStr);
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        private readonly Dictionary<int, Dictionary<int, GeneratorItemDataSo>> _itemDataListByGeneratorType = new();
        private static readonly string MinDateTimeStr = DateTime.MinValue.ToString(CultureInfo.InvariantCulture);

        #endregion
    }
}