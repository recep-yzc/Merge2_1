using System;
using UnityEngine;

namespace _Game.Development.Item.Serializable
{
    [Serializable]
    public class ItemSaveData
    {
        public SerializableVector2 coordinate;

        public int level;
        public int specialId;
        public int itemId;

        protected ItemSaveData(SerializableVector2 coordinate, int level, int itemId, int specialId)
        {
            this.coordinate = coordinate;

            this.level = level;
            this.specialId = specialId;
            this.itemId = itemId;
        }
    }

    [Serializable]
    public class SerializableVector2
    {
        public float x;
        public float y;

        public SerializableVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }

        public Vector2 ToVector2()
        {
            return new Vector2(x, y);
        }
    }
}