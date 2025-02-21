using System;
using System.Collections.Generic;
using _Game.Development.Enum.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Object.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Factory.Item
{
    public class GeneratorFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Generator generatorPrefab;
        
        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Generator;

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(generatorPrefab.gameObject);
            var iGenerator = item.GetComponent<IGenerator>();
            var iPool = item.GetComponent<IPool>();

            void DespawnPoolAction()
            {
                Despawn(item);
            }

            iPool.AddDespawnPool(DespawnPoolAction);

            var generatorItemDataSo =
                _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);

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
        private static readonly string MinDateTimeStr = DateTime.MinValue.ToString(CultureExtension.CurrentCultureInfo);

        #endregion
    }
}