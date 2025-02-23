using _Game.Development.Enum.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Object.Item;
using _Game.Development.Scriptable.Factory;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Factory.Item
{
    public class EmptyFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Empty itemPrefab;

        [Inject] private AllItemDataSo _allItemDataSo;

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.Empty;

            CreateDefaultItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateDefaultItemSaveData);
            CreateEditedItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateEditedItemSaveData);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        private GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(itemPrefab.gameObject);

            var iItem = item.GetComponent<IItem>();
            iItem.ResetParameters();

            var iEmpty = item.GetComponent<IEmpty>();
            var iPool = item.GetComponent<IPool>();

            void DespawnAction()
            {
                Despawn(item);
            }

            iPool.RegisterDespawnCallback(DespawnAction);

            var itemDataSo = _allItemDataSo.GetItemDataSoByItemSaveData(itemSaveData);

            iEmpty.SetParent(transform);
            iEmpty.SetPosition(itemSaveData.coordinate.ToVector2());
            iEmpty.SetSprite(itemDataSo.icon);

            iEmpty.SetItemDataSo(itemDataSo);
            iEmpty.FetchItemData();

            return item;
        }

        private ItemSaveData CreateDefaultItemSaveData(DefaultSaveParameters defaultSaveParameters)
        {
            var editedSave = new EditedSaveParameters(defaultSaveParameters.coordinate, defaultSaveParameters.itemDataSo);
            return CreateItemSaveData(editedSave);
        }

        private ItemSaveData CreateEditedItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            return CreateItemSaveData(editedSaveParameters);
        }

        private ItemSaveData CreateItemSaveData(EditedSaveParameters editedSaveParameters)
        {
            if (editedSaveParameters.itemDataSo is not EmptyItemDataSo emptyItemDataSo) return default;

            var jsonCoordinate = editedSaveParameters.coordinate.ToJsonVector2();
            var level = emptyItemDataSo.level;
            var itemId = emptyItemDataSo.GetItemId();
            var specialId = 0;

            return new EmptyItemSaveData(jsonCoordinate, level, itemId, specialId);
        }
    }
}