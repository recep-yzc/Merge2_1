using System;
using _Game.Development.Extension;
using _Game.Development.Item;
using UnityEngine;

namespace _Game.Development.Level
{
    [Serializable]
    public class GridData
    {
        public Vector2 coordinate;
        public ItemDataSo itemDataSo;

        public GridData(Vector2 coordinate, ItemDataSo itemDataSo)
        {
            this.coordinate = coordinate;
            this.itemDataSo = itemDataSo;
        }

        public Vector2 BottomLeft => coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => coordinate + VectorExtension.HalfSize;

        public GameObject GameObject { get; set; }
        public GridData[] NeighborGridData { get; private set; } = Array.Empty<GridData>();

        public void SetNeighborGridData(GridData[] neighborGridData)
        {
            Array.Copy(neighborGridData, NeighborGridData, NeighborGridData.Length);
        }
    }
}