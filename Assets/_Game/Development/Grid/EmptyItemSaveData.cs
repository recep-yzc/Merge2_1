using System;
using _Game.Development.Item;
using UnityEngine;

namespace _Game.Development.Grid
{
    [Serializable]
    public class EmptyItemSaveData : ItemSaveData
    {
        public EmptyItemSaveData(Vector2 coordinate, int uniqueId, int itemType, int categoryType) : base(coordinate,
            uniqueId, itemType, categoryType)
        {
        }
    }
}