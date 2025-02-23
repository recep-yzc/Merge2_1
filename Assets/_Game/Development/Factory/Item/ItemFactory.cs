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

        public static Dictionary<int, Func<DefaultSave, ItemSaveData>> CreateDefaultItemSaveDataByItemId { get; } = new();
        public static Dictionary<int, Func<EditedSave, ItemSaveData>> CreateEditedItemSaveDataByItemId { get; } = new();

        public static Dictionary<int, Func<ItemSaveData, GameObject>> CreateItemByItemId { get; } = new();

        private readonly List<GameObject> _createdItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion

        [Serializable]
        public struct EditedSave
        {
            public Vector2 coordinate;
            public ItemDataSo itemDataSo;
            public object[] Parameters;

            public EditedSave(Vector2 coordinate, ItemDataSo itemDataSo, params object[] parameters)
            {
                this.coordinate = coordinate;
                this.itemDataSo = itemDataSo;
                Parameters = parameters;
            }
        }
        
        [Serializable]
        public struct DefaultSave
        {
            public Vector2 coordinate;
            public ItemDataSo itemDataSo;

            public DefaultSave(Vector2 coordinate, ItemDataSo itemDataSo)
            {
                this.coordinate = coordinate;
                this.itemDataSo = itemDataSo;
            }
        }
    }
}