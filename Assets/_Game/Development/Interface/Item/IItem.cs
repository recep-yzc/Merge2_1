using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Interface.Item
{
    public interface IItem
    {
        public void SetParent(Transform parent);
        public void SetPosition(Vector2 position);
        public void SetSprite(Sprite icon);
        
        
        public void SetItemDataSo(ItemDataSo itemDataSo);
        public void FetchItemData();
    }
}