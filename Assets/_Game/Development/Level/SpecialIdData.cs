using System;
using System.Collections.Generic;

namespace _Game.Development.Level
{
    [Serializable]
    public class SpecialIdData
    {
        public int specialId;
        public List<Item.ItemDataSo> itemItemDataSoList = new();
    }
}