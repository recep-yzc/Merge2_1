using System;
using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Scriptable.Factory
{
    [Serializable]
    public struct EditedSaveParameters
    {
        public Vector2 coordinate;
        public ItemDataSo itemDataSo;
        public object[] Parameters;

        public EditedSaveParameters(Vector2 coordinate, ItemDataSo itemDataSo, params object[] parameters)
        {
            this.coordinate = coordinate;
            this.itemDataSo = itemDataSo;
            Parameters = parameters;
        }
    }
}