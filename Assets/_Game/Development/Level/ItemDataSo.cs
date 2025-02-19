using System.Collections.Generic;
using UnityEngine;

namespace _Game.Development.Level
{
    [CreateAssetMenu(fileName = "ItemDataSo" ,menuName = "Game/Level/ItemDataSo")]
    public class ItemDataSo : ScriptableObject
    {
        public List<ItemTypeData> itemDataSoList = new();
        
        
    }
}