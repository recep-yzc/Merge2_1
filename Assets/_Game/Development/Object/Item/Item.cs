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
    public abstract class Item : MonoBehaviour, IItem, IPool, IScaleUpDown, IMoveable
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        #region Unity Action

        protected virtual void OnDestroy()
        {
            DisposeScaleUpDownToken();
            DisposeMoveToken();
            DisposeJumpToken();
        }

        #endregion

        #region Parameters

        protected readonly float DragMoveSpeed = 15f;
        protected Vector2 SelfCoordinate;
        protected ItemDataSo ItemDataSo { get; private set; }

        private Action _backPoolAction;
        private CancellationTokenSource _cancellationScaleUpDownTokenSource;
        private CancellationTokenSource _cancellationMoveTokenSource;
        private CancellationTokenSource _cancellationJumpTokenSource;

        #endregion

        #region Item

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
            SelfCoordinate = position;
        }

        public void SetSprite(Sprite icon)
        {
            sprIcon.sprite = icon;
        }

        public virtual void ResetParameters()
        {
            SetSpriteOrder(0);
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

        public virtual void LevelUp()
        {
        }

        public abstract ItemSaveData CreateItemSaveData();

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
            if (_cancellationScaleUpDownTokenSource is not { IsCancellationRequested: false }) return;

            _cancellationScaleUpDownTokenSource.Cancel();
            _cancellationScaleUpDownTokenSource.Dispose();
            _cancellationScaleUpDownTokenSource = null;
        }

        #endregion

        #region Move

        public async UniTask MoveAsync(Vector2 coordinate, MoveDataSo moveDataSo)
        {
            SelfCoordinate = coordinate;
            DisposeMoveToken();

            _cancellationMoveTokenSource = new CancellationTokenSource();
            await AbilityExtension.MoveHandle(transform, coordinate, moveDataSo, _cancellationMoveTokenSource.Token);
        }

        private void DisposeMoveToken()
        {
            if (_cancellationMoveTokenSource is not { IsCancellationRequested: false }) return;

            _cancellationMoveTokenSource.Cancel();
            _cancellationMoveTokenSource.Dispose();
            _cancellationMoveTokenSource = null;
        }

        private void DisposeJumpToken()
        {
            if (_cancellationJumpTokenSource is not { IsCancellationRequested: false }) return;

            _cancellationJumpTokenSource.Cancel();
            _cancellationJumpTokenSource.Dispose();
            _cancellationJumpTokenSource = null;
        }

        #endregion

        #endregion
    }
}