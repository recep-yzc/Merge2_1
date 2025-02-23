using System;
using System.Collections.Generic;
using _Game.Development.Enum.Item;
using _Game.Development.Scriptable.Factory;
using _Game.Development.Serializable.Item;
using UnityEngine;

namespace _Game.Development.Factory.Item
{
    public abstract class ItemFactory : MonoBehaviour
    {
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

        protected void Despawn(GameObject prefab)
        {
            prefab.SetActive(false);
        }

        #region Parameters

        public static Dictionary<int, Func<DefaultSaveParameters, ItemSaveData>> CreateDefaultItemSaveDataByItemId { get; } =
            new();

        public static Dictionary<int, Func<EditedSaveParameters, ItemSaveData>> CreateEditedItemSaveDataByItemId { get; } = new();

        public static Dictionary<int, Func<ItemSaveData, GameObject>> CreateItemByItemId { get; } = new();

        private readonly List<GameObject> _createdItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion
    }
}