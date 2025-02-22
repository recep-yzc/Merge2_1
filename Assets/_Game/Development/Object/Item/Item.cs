using System;
using System.Threading;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public abstract class Item : MonoBehaviour, IItem, IPool, IClickable, IDraggable, IScaleUpDown, IMoveable
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        #region Unity Action

        private void OnDestroy()
        {
            DisposeScaleUpDownToken();
            DisposeMoveToken();
        }

        #endregion

        #region Parameters

        protected readonly float DragMoveSpeed = 15f;
        protected ItemDataSo ItemDataSo { get; set; }

        private Action _backPoolAction;
        private CancellationTokenSource _cancellationScaleUpDownToken;
        private CancellationTokenSource _cancellationMoveToken;

        #endregion

        #region Item

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetSprite(Sprite icon)
        {
            sprIcon.sprite = icon;
        }

        public virtual void SetItemDataSo(ItemDataSo itemDataSo)
        {
            ItemDataSo = itemDataSo;
        }

        public virtual void FetchItemData()
        {
        }

        protected void SetSpriteOrder(int order)
        {
            sprIcon.sortingOrder = order;
        }

        #endregion

        #region Pool

        public void AddDespawnPool(Action action)
        {
            _backPoolAction += action;
        }

        public void RemoveDespawnPool(Action action)
        {
            _backPoolAction -= action;
        }

        public void PlayDespawnPool()
        {
            _backPoolAction?.Invoke();
        }

        #endregion

        #region Clickable

        public abstract void MouseDown();
        public abstract void MouseUp();
        public abstract void Drag(Vector2 vector2);

        #endregion

        #region Ability

        #region ScaleUpDown

        public UniTaskVoid ScaleUpDownAsync(ScaleUpDownDataSo scaleUpDownDataSo)
        {
            DisposeScaleUpDownToken();

            _cancellationScaleUpDownToken = new CancellationTokenSource();
            return AbilityExtension.ScaleUpDownHandle(transform, scaleUpDownDataSo,
                _cancellationScaleUpDownToken.Token);
        }

        private void DisposeScaleUpDownToken()
        {
            _cancellationScaleUpDownToken?.Cancel();
            _cancellationScaleUpDownToken?.Dispose();
        }

        #endregion

        #region Move

        public async UniTask MoveAsync(Vector2 coordinate, MoveDataSo moveDataSo)
        {
            DisposeMoveToken();

            _cancellationMoveToken = new CancellationTokenSource();
            await AbilityExtension.MoveHandle(transform, coordinate, moveDataSo,
                _cancellationMoveToken.Token);
        }

        private void DisposeMoveToken()
        {
            _cancellationMoveToken?.Cancel();
            _cancellationMoveToken?.Dispose();
        }

        #endregion

        #endregion
    }
}