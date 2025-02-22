using System;
using System.Threading;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
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

        protected virtual void OnDestroy()
        {
            DisposeScaleUpDownToken();
            DisposeMoveToken();
        }

        #endregion

        #region Parameters

        protected readonly float DragMoveSpeed = 15f;
        protected ItemDataSo ItemDataSo { get; set; }

        private Action _backPoolAction;
        private CancellationTokenSource _cancellationScaleUpDownTokenSource;
        private CancellationTokenSource _cancellationMoveTokenSource;

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
        
        protected void SetSpriteOrder(int order)
        {
            sprIcon.sortingOrder = order;
        }

        
        public virtual void SetItemDataSo(ItemDataSo itemDataSo)
        {
            ItemDataSo = itemDataSo;
        }

        public virtual void FetchItemData()
        {
            
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

            _cancellationScaleUpDownTokenSource = new CancellationTokenSource();
            return AbilityExtension.ScaleUpDownHandle(transform, scaleUpDownDataSo,
                _cancellationScaleUpDownTokenSource.Token);
        }

        private void DisposeScaleUpDownToken()
        {
            _cancellationScaleUpDownTokenSource?.Cancel();
            _cancellationScaleUpDownTokenSource?.Dispose();
        }

        #endregion

        #region Move

        public async UniTask MoveAsync(Vector2 coordinate, MoveDataSo moveDataSo)
        {
            DisposeMoveToken();

            _cancellationMoveTokenSource = new CancellationTokenSource();
            await AbilityExtension.MoveHandle(transform, coordinate, moveDataSo,
                _cancellationMoveTokenSource.Token);
        }

        private void DisposeMoveToken()
        {
            _cancellationMoveTokenSource?.Cancel();
            _cancellationMoveTokenSource?.Dispose();
        }

        #endregion

        #endregion
    }
}