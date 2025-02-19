using System;
using System.Collections.Generic;
using _Game.Development.Item;

namespace _Game.Development.Level
{
    [Serializable]
    public class ItemTypeData
    {
        public ItemType itemType;
        public List<SpecialIdData> specialIdDataList = new();
    }
}