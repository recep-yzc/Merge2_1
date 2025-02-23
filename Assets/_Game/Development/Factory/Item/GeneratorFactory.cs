using System;
using _Game.Development.Enum.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Object.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Factory;
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

            CreateDefaultItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateDefaultItemSaveData);
            CreateEditedItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateEditedItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        private GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(generatorPrefab.gameObject);

            var iItem = item.GetComponent<IItem>();
            iItem.ResetParameters();

            var iGenerator = item.GetComponent<IGenerator>();
            var iPool = item.GetComponent<IPool>();

            void DespawnAction()
            {
                Despawn(item);
            }

            iPool.RegisterDespawnCallback(DespawnAction);

            var generatorItemSaveData = (GeneratorItemSaveData)itemSaveData;
            var itemDataSo = _allItemDataSo.GetItemDataSoByItemSaveData(itemSaveData);

            iGenerator.SetParent(transform);
            iGenerator.SetPosition(itemSaveData.coordinate.ToVector2());
            iGenerator.SetSprite(itemDataSo.icon);

            iGenerator.SetLastUsingDate(generatorItemSaveData.lastUsingDate);
            iGenerator.SetItemDataSo(itemDataSo);
            iGenerator.FetchItemData();

            return item;
        }

        private ItemSaveData CreateDefaultItemSaveData(DefaultSaveParameters defaultSaveParameters)
        {
            var editedSave = new EditedSaveParameters(defaultSaveParameters.coordinate,
                defaultSaveParameters.itemDataSo, MinDateTimeStr);
            return CreateItemSaveData(editedSave);
        }

        private ItemSaveData CreateEditedItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            return CreateItemSaveData(editedSaveParameters);
        }

        private ItemSaveData CreateItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            if (editedSaveParameters.itemDataSo is not GeneratorItemDataSo generatorItemDataSo) return default;

            var jsonCoordinate = editedSaveParameters.coordinate.ToJsonVector2();
            var level = generatorItemDataSo.level;
            var itemId = generatorItemDataSo.GetItemId();
            var specialId = generatorItemDataSo.GetSpecialId();
            var lastUsingDate = (string)editedSaveParameters.Parameters[0];

            return new GeneratorItemSaveData(jsonCoordinate, level, itemId, specialId, lastUsingDate);
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        private static readonly string MinDateTimeStr = DateTime.MinValue.DateTimeToString();

        #endregion
    }
}