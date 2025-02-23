using System.Threading;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Factory;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Product : Item, IProduct, IDraggable, IClickable, IScaleUpDown
    {
        #region Unity Action

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeScaleUpDownTokenSource();
        }

        #endregion

        #region Parameters

        private readonly float _dragMoveSpeed = 15f;
        private ProductItemDataSo _productItemDataSo;

        private CancellationTokenSource _scaleUpDownCancellationTokenSource;

        #endregion

        #region Item

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            _productItemDataSo = (ProductItemDataSo)itemDataSo;
        }

        public override ItemSaveData CreateEditedItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(transform.position);

            var coordinate = gridData.Coordinate;
            var itemDataSo = gridData.itemDataSo;
            var itemId = itemDataSo.GetItemId();

            var createEditedItemSaveDataFunc = ItemFactory.CreateEditedItemSaveDataByItemId[itemId];
            var editedItemSaveParameters = new EditedSaveParameters(coordinate, itemDataSo);
            return createEditedItemSaveDataFunc.Invoke(editedItemSaveParameters);
        }

        #endregion

        #region Ability

        #region Draggable

        public void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * _dragMoveSpeed);
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

        #region ScaleUpDown

        public UniTaskVoid ScaleUpDownAsync(ScaleUpDownDataSo scaleUpDownDataSo)
        {
            DisposeScaleUpDownTokenSource();
            NewScaleUpTokenSource();

            return AbilityExtension.ScaleUpDownHandle(transform, scaleUpDownDataSo,
                _scaleUpDownCancellationTokenSource.Token);
        }

        private void NewScaleUpTokenSource()
        {
            _scaleUpDownCancellationTokenSource = new CancellationTokenSource();
        }

        private void DisposeScaleUpDownTokenSource()
        {
            if (_scaleUpDownCancellationTokenSource is not { IsCancellationRequested: false }) return;

            _scaleUpDownCancellationTokenSource.Cancel();
            _scaleUpDownCancellationTokenSource.Dispose();
            _scaleUpDownCancellationTokenSource = null;
        }

        #endregion

        #endregion
    }
}