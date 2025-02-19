using System;
using System.Collections.Generic;
using _Game.Development.Level;
using UnityEngine;
using Zenject;

namespace _Game.Development.Item
{
    public abstract class ItemFactory : MonoBehaviour
    {
        public abstract GameObject CreateItem(GridData gridData);
        public abstract ItemSaveData CreateItemSaveData(GridData gridData);
        
        protected GameObject GetOrCreateItemInPool(ref List<GameObject> createdGameObjectList, GameObject prefab)
        {
            var find = createdGameObjectList.Find(x => !x.activeInHierarchy);
            if (find != null)
            {
                find.SetActive(true);
                return find;
            }

            find = _diContainer.InstantiatePrefab(prefab);
            createdGameObjectList.Add(find);

            return find;
        }

        #region Parameters

        [Inject] private DiContainer _diContainer;

        public static Dictionary<int, Func<GridData, ItemSaveData>> CreateItemSaveDataByCategoryType { get; } =
            new();

        public static Dictionary<int, Func<GridData, GameObject>> CreateItemByCategoryType { get; } = new();

        protected List<GameObject> CreatedItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion
    }
}