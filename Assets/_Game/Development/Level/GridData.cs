using System;
using _Game.Development.Extension;
using UnityEngine;

namespace _Game.Development.Level
{
    [Serializable]
    public class GridData
    {
        public Vector2 coordinate;
        public Item.ItemDataSo itemDataSo;

        public GridData(Vector2 coordinate, Item.ItemDataSo itemDataSo)
        {
            this.coordinate = coordinate;
            this.itemDataSo = itemDataSo;
            NeighborGridData = Array.Empty<GridData>();
        }

        public Vector2 BottomLeft => coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => coordinate + VectorExtension.HalfSize;

        public GameObject GameObject { get; set; }
        public GridData[] NeighborGridData { get; set; } 

        public void SetNeighborGridData(GridData[] neighborGridData)
        {
            Array.Copy(neighborGridData, NeighborGridData, NeighborGridData.Length);
        }
    }
}