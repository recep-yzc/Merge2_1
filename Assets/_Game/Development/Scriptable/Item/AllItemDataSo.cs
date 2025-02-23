using System.Collections.Generic;
using System.Linq;
using _Game.Development.Enum.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;
using Zenject;

namespace _Game.Development.Scriptable.Item
{
    [CreateAssetMenu(fileName = "AllItemDataSo", menuName = "Game/Core/Board/Item/AllItemDataSo")]
    public class AllItemDataSo : ScriptableObjectInstaller<AllItemDataSo>
    {
        public List<ItemTypeData> itemDataSoList = new();

        public ItemDataSo GetEmptyItemDataSo()
        {
            return itemDataSoList.First().specialIdDataList.First().itemItemDataSoList.First();
        }

        public ItemDataSo GetItemDataSoByItemSaveData(ItemSaveData itemSaveData)
        {
            return GetItemDataByIds(itemSaveData.level, itemSaveData.itemId, itemSaveData.specialId);
        }

        public ItemDataSo GetItemDataByItemDataSo(ItemDataSo itemDataSo)
        {
            return GetItemDataByIds(itemDataSo.level, itemDataSo.GetItemId(), itemDataSo.GetSpecialId());
        }

        private ItemDataSo GetItemDataByIds(int level, int itemId, int specialId)
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