using _Game.Development.Interface.Item;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Product : Item, IProduct
    {
        public override void OnDown()
        {
            SetSpriteOrder(1);
        }

        public override void OnUp()
        {
            SetSpriteOrder(0);
        }

        public override void OnDrag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }
        
    }
}