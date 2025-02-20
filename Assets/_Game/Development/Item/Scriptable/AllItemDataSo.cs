using System.Collections.Generic;
using System.Linq;
using _Game.Development.Board.Edit.Serializable;
using UnityEngine;

namespace _Game.Development.Item.Scriptable
{
    [CreateAssetMenu(fileName = "AllItemDataSo", menuName = "Game/Core/Board/Item/AllItemDataSo")]
    public class AllItemDataSo : ScriptableObject
    {
        public List<ItemTypeData> itemDataSoList = new();

        public ItemDataSo GetEmptyItemDataSo()
        {
            return itemDataSoList.First().specialIdDataList.First().itemItemDataSoList.First();
        }
    }
}