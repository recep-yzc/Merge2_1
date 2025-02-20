using System;
using System.Collections.Generic;
using _Game.Development.Enum.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using UnityEngine;

namespace _Game.Development.Factory.Item
{
    public abstract class ItemFactory : MonoBehaviour
    {
        protected abstract GameObject CreateItem(ItemSaveData itemSaveData);
        protected abstract ItemSaveData CreateItemSaveData(SerializableVector2 coordinate, ItemDataSo itemDataSo);

        protected GameObject GetOrCreateItemInPool(GameObject prefab)
        {
            var find = _createdItemList.Find(x => !x.activeInHierarchy);
            if (find != null)
            {
                find.SetActive(true);
                return find;
            }

            find = Instantiate(prefab);
            _createdItemList.Add(find);

            return find;
        }

        protected void BackPool(GameObject prefab)
        {
            prefab.SetActive(false);
        }

        #region Parameters

        public static Dictionary<int, Func<SerializableVector2, ItemDataSo, ItemSaveData>> CreateItemSaveDataByItemId
        {
            get;
        } = new();

        public static Dictionary<int, Func<ItemSaveData, GameObject>> CreateItemByItemId { get; } = new();
        private readonly List<GameObject> _createdItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion
    }
}