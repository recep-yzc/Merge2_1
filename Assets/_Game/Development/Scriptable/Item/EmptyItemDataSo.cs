using _Game.Development.Enum.Item;
using UnityEngine;

namespace _Game.Development.Scriptable.Item
{
    [CreateAssetMenu(fileName = "EmptyItemDataSo", menuName = "Game/Board/Item/Empty/Data")]
    public class EmptyItemDataSo : ItemDataSo
    {
        public EmptyItemDataSo()
        {
            itemType = ItemType.Empty;
        }

        public override int GetSpecialId()
        {
            return 0;
        }
    }
}