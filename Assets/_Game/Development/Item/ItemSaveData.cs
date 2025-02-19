using System;
using UnityEngine;

namespace _Game.Development.Item
{
    [Serializable]
    public abstract class ItemSaveData
    {
        public Vector2 coordinate;

        public int uniqueId;
        public int itemType;
        public int categoryType;

        protected ItemSaveData(Vector2 coordinate, int uniqueId, int itemType, int categoryType)
        {
            this.coordinate = coordinate;
            this.uniqueId = uniqueId;
            this.itemType = itemType;
            this.categoryType = categoryType;
        }
    }
}