using System;
using System.Collections.Generic;
using _Game.Development.Serializable.Item;

namespace _Game.Development.Enum.Item
{
    [Serializable]
    public class ItemTypeData
    {
        public ItemType itemType;
        public List<SpecialIdData> specialIdDataList = new();
    }
}