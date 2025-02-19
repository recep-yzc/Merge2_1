using System;
using _Game.Development.Item;
using UnityEngine;

namespace _Game.Development.Level
{
    [Serializable]
    public class LevelGridData
    {
        public Vector2 coordinate;
        public ItemDataSo itemDataSo;

        public LevelGridData(Vector2 coordinate)
        {
            this.coordinate = coordinate;
        }
    }
}