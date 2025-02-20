using UnityEngine;

namespace _Game.Development.Item
{
    public class Generator : Item, IGenerator, IClickable
    {
        public void OnDown()
        {
            SetSpriteOrder(1);
        }

        public void OnUp(bool wasIsTransferred)
        {
            SetSpriteOrder(0);
        }

        public void OnDrag(Vector2 vector2)
        {
            transform.position = vector2;
        }
    }
}