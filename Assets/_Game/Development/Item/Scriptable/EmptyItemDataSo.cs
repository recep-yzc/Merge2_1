using UnityEngine;

namespace _Game.Development.Item.Scriptable
{
    [CreateAssetMenu(fileName = "EmptyItemDataSo", menuName = "Game/Core/Board/Item/Empty/Data")]
    public class EmptyItemDataSo : ItemDataSo
    {
        public EmptyItemDataSo()
        {
            itemType = ItemType.None;
        }

        public override int GetSpecialId()
        {
            return 0;
        }
    }
}