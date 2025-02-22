using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Product : Item, IProduct, IDraggable, IClickable
    {
        #region Draggable

        public void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Item

        public override ItemSaveData CreateItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(transform.position);
            return new ProductItemSaveData(gridData.coordinate.ToJsonVector2(), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId());
        }

        #endregion

        #region Clickable

        public void MouseDown()
        {
            SetSpriteOrder(1);
        }

        public void MouseUp()
        {
            SetSpriteOrder(0);
        }

        #endregion
    }
}