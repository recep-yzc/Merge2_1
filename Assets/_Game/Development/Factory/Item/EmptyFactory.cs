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
    public class EmptyFactory : ItemFactory
    {
        [Header("References")] [SerializeField]
        private Empty itemPrefab;

        [Inject] private AllItemDataSo _allItemDataSo;

        #region Unity Action

        private void Awake()
        {
            ItemType = ItemType.None;

            CreateItemSaveDataByItemId.TryAdd(ItemType.ToInt(), CreateItemSaveData<EmptyItemSaveData>);
            CreateItemByItemId.TryAdd(ItemType.ToInt(), CreateItem);
        }

        #endregion

        protected override GameObject CreateItem(ItemSaveData itemSaveData)
        {
            var item = GetOrCreateItemInPool(itemPrefab.gameObject);
            var iEmpty = item.GetComponent<IEmpty>();

            var generatorItemSaveData = (EmptyItemSaveData)itemSaveData;
            var itemDataSo = _allItemDataSo.GetItemDataByIds(itemSaveData.itemId, itemSaveData.specialId, itemSaveData.level);

            iEmpty.SetParent(transform);
            iEmpty.SetPosition(itemSaveData.coordinate.ToVector2());
            iEmpty.SetSprite(itemDataSo.icon);

            iEmpty.SetItemDataSo(itemDataSo);
            iEmpty.FetchItemData();

            return item;
        }

        protected override T CreateItemSaveData<T>(SerializableVector2 coordinate, ItemDataSo itemDataSo)
        {
            var dataSo = (EmptyItemDataSo)itemDataSo;
            return new EmptyItemSaveData(coordinate, dataSo.level, dataSo.itemType.ToInt(), 0) as T;
        }
    }
}