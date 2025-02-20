using System;
using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Serializable;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Scriptable;
using UnityEngine;
using Zenject;

namespace _Game.Development.Item
{
    public abstract class ItemFactory : MonoBehaviour
    {
        protected abstract GameObject CreateItem(ItemSaveData itemSaveData);
        protected abstract ItemSaveData CreateItemSaveData(SerializableVector2 coordinate, ItemDataSo itemDataSo);

        protected GameObject GetOrCreateItemInPool(GameObject prefab)
        {
            var find = CreatedItemList.Find(x => !x.activeInHierarchy);
            if (find != null)
            {
                find.SetActive(true);
                return find;
            }

            find = _diContainer.InstantiatePrefab(prefab);
            CreatedItemList.Add(find);

            return find;
        }

        protected void BackPool(GameObject prefab)
        {
            prefab.SetActive(false);
        }

        #region Parameters

        [Inject] private DiContainer _diContainer;

        public static Dictionary<int, Func<SerializableVector2, ItemDataSo, ItemSaveData>> CreateItemSaveDataByItemId
        {
            get;
        } = new();

        public static Dictionary<int, Func<ItemSaveData, GameObject>> CreateItemByItemId { get; } = new();

        protected List<GameObject> CreatedItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion
    }
}