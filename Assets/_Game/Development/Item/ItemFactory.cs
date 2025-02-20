using System;
using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Grid.Serializable;
using _Game.Development.Item.Serializable;
using UnityEngine;
using Zenject;

namespace _Game.Development.Item
{
    public abstract class ItemFactory : MonoBehaviour
    {
        public abstract GameObject CreateItem(GridData gridData);
        public abstract ItemSaveData CreateItemSaveData(GridInspectorData gridInspectorData);

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

        public static Dictionary<int, Func<GridInspectorData, ItemSaveData>> CreateItemSaveDataBySpecialId { get; } =
            new();

        public static Dictionary<int, Func<GridData, GameObject>> CreateItemBySpecialId { get; } = new();

        protected List<GameObject> CreatedItemList = new();
        protected ItemType ItemType { get; set; }

        #endregion
    }
}