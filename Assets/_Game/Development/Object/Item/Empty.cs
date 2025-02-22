using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Empty : Item, IEmpty
    {
        #region Draggable

        public override void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Parameters

        private int _spawnAmount;
        private EmptyItemDataSo _emptyItemDataSo;

        #endregion

        #region Item

        public override ItemSaveData GetItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(transform.position);
            return new EmptyItemSaveData(new SerializableVector2(gridData.coordinate), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId());
        }

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            base.SetItemDataSo(itemDataSo);
            _emptyItemDataSo = (EmptyItemDataSo)itemDataSo;
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