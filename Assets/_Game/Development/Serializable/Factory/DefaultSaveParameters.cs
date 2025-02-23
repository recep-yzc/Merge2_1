using System;
using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Serializable.Factory
{
    [Serializable]
    public struct DefaultSaveParameters
    {
        public Vector2 coordinate;
        public ItemDataSo itemDataSo;

        public DefaultSaveParameters(Vector2 coordinate, ItemDataSo itemDataSo)
        {
            this.coordinate = coordinate;
            this.itemDataSo = itemDataSo;
        }
    }
}