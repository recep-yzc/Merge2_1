using System;
using _Game.Development.Extension.Static;
using _Game.Development.Item.Scriptable;
using UnityEngine;

namespace _Game.Development.Grid.Serializable
{
    [Serializable]
    public class GridData
    {
        public GridData(Vector2 coordinate, GameObject gameObject, ItemDataSo itemDataSo)
        {
            Coordinate = coordinate;
            GameObject = gameObject;
            ItemDataSo = itemDataSo;

            NeighborGridData = Array.Empty<GridData>();
        }

        public void SetNeighborGridData(GridData[] neighborGridData)
        {
            Array.Copy(neighborGridData, NeighborGridData, NeighborGridData.Length);
        }

        #region Parameters

        public Vector2 Coordinate { get; set; }
        public ItemDataSo ItemDataSo { get; set; }
        public GameObject GameObject { get; set; }
        public GridData[] NeighborGridData { get; set; }
        public Vector2 BottomLeft => Coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => Coordinate + VectorExtension.HalfSize;

        #endregion
    }
}