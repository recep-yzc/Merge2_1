using System;
using System.Collections.Generic;
using _Game.Development.Item;
using Sirenix.OdinInspector;

namespace _Game.Development.Board
{
    [Serializable]
    public class BoardSaveData
    {
        [ShowInInspector] public List<ItemSaveData> ItemSaveDataList = new();
    }
}