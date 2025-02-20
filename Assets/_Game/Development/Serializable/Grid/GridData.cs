using System;
using _Game.Development.Scriptable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Serializable.Grid
{
    [Serializable]
    public class GridData
    {
        public Vector2 coordinate;
        public GameObject gameObject;
        public ItemDataSo itemDataSo;

        [NonSerialized] public GridData[] NeighborGridDataList;

        public GridData(Vector2 coordinate, GameObject gameObject, ItemDataSo itemDataSo,
            GridData[] neighborGridDataList)
        {
            this.coordinate = coordinate;
            this.gameObject = gameObject;
            this.itemDataSo = itemDataSo;
            NeighborGridDataList = neighborGridDataList;
        }

        public Vector2 BottomLeft => coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => coordinate + VectorExtension.HalfSize;

        public T GetComponent<T>()
        {
            return gameObject.GetComponent<T>();
        }
    }
}