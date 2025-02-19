using System;
using UnityEngine;

namespace _Game.Development.Level
{
    [Serializable]
    public record GridJsonData
    {
        public Vector2 coordinate;
        public int uniqueId;
        public int specialId;
        public int itemId;

        public GridJsonData(Vector2 coordinate, int uniqueId, int specialId, int itemId)
        {
            this.coordinate = coordinate;

            this.uniqueId = uniqueId;
            this.specialId = specialId;
            this.itemId = itemId;
        }
    }
}