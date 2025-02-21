using _Game.Development.Interface.Item;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Generator : Item, IGenerator
    {
        public override void OnDown()
        {
            base.OnDown();
            SetSpriteOrder(1);
        }

        public override void OnUp()
        {
            base.OnUp();
            SetSpriteOrder(0);
        }

        public override void OnDrag(Vector2 vector2)
        {
            if (!IsHolding) return;
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }
    }
}