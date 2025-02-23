using System;
using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Scriptable.Factory
{
    [Serializable]
    public struct DefaultSave
    {
        public Vector2 coordinate;
        public ItemDataSo itemDataSo;

        public DefaultSave(Vector2 coordinate, ItemDataSo itemDataSo)
        {
            this.coordinate = coordinate;
            this.itemDataSo = itemDataSo;
        }
    }
}