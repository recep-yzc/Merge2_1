using System;
using System.Linq;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Serializable.Grid
{
    [Serializable]
    public class GridData
    {
        public Vector2 coordinate;
        public GameObject item;
        public ItemDataSo itemDataSo;

        [NonSerialized] public GridData[] NeighborGridDataArray;

        public GridData(Vector2 coordinate, GameObject item, ItemDataSo itemDataSo)
        {
            this.coordinate = coordinate;
            this.item = item;
            this.itemDataSo = itemDataSo;
        }

        public bool IsEmpty() => item == null;

        public Vector2 BottomLeft => coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => coordinate + VectorExtension.HalfSize;

        public void CopyNeighborGridData(GridData[] gridDataArray)
        {
            NeighborGridDataArray = gridDataArray.ToArray();
        }

        public T GetComponent<T>()
        {
            return item.GetComponent<T>();
        }
    }
}