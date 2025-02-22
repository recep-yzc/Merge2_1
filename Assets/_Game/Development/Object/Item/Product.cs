using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Product : Item, IProduct
    {
        #region Parameters

        private ProductItemDataSo _productItemDataSo;

        #endregion

        #region Item

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            base.SetItemDataSo(itemDataSo);
            _productItemDataSo = (ProductItemDataSo)itemDataSo;
        }

        #endregion

        #region Draggable

        public override void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Clickable

        public override void MouseDown()
        {
            SetSpriteOrder(1);
        }

        public override void MouseUp()
        {
            SetSpriteOrder(0);
        }

        #endregion
    }
}