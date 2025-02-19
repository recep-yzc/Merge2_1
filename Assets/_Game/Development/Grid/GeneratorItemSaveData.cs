using System;
using _Game.Development.Item;
using UnityEngine;

namespace _Game.Development.Grid
{
    [Serializable]
    public class GeneratorItemSaveData : ItemSaveData
    {
        public int spawnAmount;
        public string lastUseTime;

        public GeneratorItemSaveData(Vector2 coordinate, int uniqueId, int itemType, int categoryType, int spawnAmount,
            string lastUseTime) : base(coordinate, uniqueId, itemType, categoryType)
        {
            this.spawnAmount = spawnAmount;
            this.lastUseTime = lastUseTime;
        }
    }
}