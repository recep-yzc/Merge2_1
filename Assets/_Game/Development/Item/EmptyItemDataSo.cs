using UnityEngine;

namespace _Game.Development.Item
{
    [CreateAssetMenu(fileName = "EmptyItemDataSo", menuName = "Game/Item/Empty/Data")]
    public class EmptyItemDataSo : ItemDataSo
    {
        public EmptyItemDataSo()
        {
            itemType = ItemType.None;
        }
    }
}