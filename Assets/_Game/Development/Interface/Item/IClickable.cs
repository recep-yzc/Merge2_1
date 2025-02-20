using UnityEngine;

namespace _Game.Development.Interface.Item
{
    public interface IClickable
    {
        public void OnDown();
        public void OnUp(bool wasIsTransferred);
        public void OnDrag(Vector2 vector2);
    }
}