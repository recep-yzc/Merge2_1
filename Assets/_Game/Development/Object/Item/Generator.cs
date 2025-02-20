using _Game.Development.Interface.Item;
using UnityEngine;

namespace _Game.Development.Object.Item
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
            ScaleUpDown();
        }

        public void OnDrag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }
    }
}