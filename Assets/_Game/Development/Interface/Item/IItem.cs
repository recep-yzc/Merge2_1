using System;
using UnityEngine;

namespace _Game.Development.Interface.Item
{
    public interface IItem
    {
        public void SetParent(Transform parent);
        public void SetPosition(Vector2 position);
        public void SetSprite(Sprite icon);
    }
}