using System;
using UnityEngine;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public record GridJsonData
    {
        public Vector2 coordinate;
        public int level;
        public int specialId;
        public int itemId;

        public GridJsonData(Vector2 coordinate, int level, int specialId, int itemId)
        {
            this.coordinate = coordinate;

            this.level = level;
            this.specialId = specialId;
            this.itemId = itemId;
        }
    }
}