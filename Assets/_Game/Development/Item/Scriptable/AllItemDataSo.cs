using System.Collections.Generic;
using System.Linq;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Extension.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Item.Scriptable
{
    [CreateAssetMenu(fileName = "AllItemDataSo", menuName = "Game/Core/Board/Item/AllItemDataSo")]
    public class AllItemDataSo : ScriptableObjectInstaller<AllItemDataSo>
    {
        public List<ItemTypeData> itemDataSoList = new();

        public ItemDataSo GetEmptyItemDataSo()
        {
            return itemDataSoList.First().specialIdDataList.First().itemItemDataSoList.First();
        }

        public ItemDataSo GetItemDataByIds(int itemId, int specialId, int level)
        {
            var itemTypeData = itemDataSoList.First(x => x.itemType.ToInt() == itemId);
            var specialIdData = itemTypeData.specialIdDataList.First(x => x.specialId == specialId);
            return specialIdData.itemItemDataSoList.First(x => x.level == level);
        }

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
}