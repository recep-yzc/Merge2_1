using System;
using System.Collections.Generic;
using _Game.Development.Item.Scriptable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class SpecialIdData
    {
        public int specialId;
        public List<ItemDataSo> itemItemDataSoList = new();
    }
}