using System;
using _Game.Development.Extension.Static;
using _Game.Development.Item.Scriptable;
using JetBrains.Annotations;
using UnityEngine;

namespace _Game.Development.Grid.Serializable
{
    [Serializable]
    public class GridData
    {
        public Vector2 Coordinate;
        public GameObject GameObject;
        public ItemDataSo ItemDataSo;

        [NonSerialized] public GridData[] NeighborGridData;

        public Vector2 BottomLeft => Coordinate - VectorExtension.HalfSize;
        public Vector2 TopRight => Coordinate + VectorExtension.HalfSize;

        public T GetComponent<T>()
        {
            return GameObject.GetComponent<T>();
        }
    }
}