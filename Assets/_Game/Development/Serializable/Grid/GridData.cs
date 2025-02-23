using System;
using System.Linq;
using _Game.Development.Enum.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Serializable.Grid
{
    [Serializable]
    public class GridData
    {
        public readonly Vector2 Coordinate;
        public GameObject item;
        public ItemDataSo itemDataSo;

        [NonSerialized] public GridData[] NeighborGridDataArray;

        public GridData(Vector2 coordinate, GameObject item, ItemDataSo itemDataSo)
        {
            Coordinate = coordinate;
            this.item = item;
            this.itemDataSo = itemDataSo;
        }

        public Vector2 BottomLeft => Coordinate - VectorExtension.Parameters.HalfSize;
        public Vector2 TopRight => Coordinate + VectorExtension.Parameters.HalfSize;

        public bool IsEmpty()
        {
            return itemDataSo.itemType is ItemType.Empty;
        }

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