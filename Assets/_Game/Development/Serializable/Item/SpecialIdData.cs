using System;
using System.Collections.Generic;
using _Game.Development.Scriptable.Item;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class SpecialIdData
    {
        public int specialId;
        public List<ItemDataSo> itemItemDataSoList = new();
    }
}