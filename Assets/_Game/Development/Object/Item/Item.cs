using System;
using System.Threading;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public abstract class Item : MonoBehaviour, IItem, IPool, IMoveable
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        #region Unity Action

        protected virtual void OnDestroy()
        {
            DisposeMoveTokenSource();
        }

        #endregion

        #region Parameters

        private CancellationTokenSource _moveCancellationTokenSource;
        protected Vector2 SelfCoordinate;
        private Action _backPoolAction;

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

        protected void SetSpriteOrder(sbyte order)
        {
            sprIcon.sortingOrder = order;
        }

        public abstract void SetItemDataSo(ItemDataSo itemDataSo);

        public virtual void FetchItemData()
        {
        }

        public virtual void LevelUp()
        {
        }

        public abstract ItemSaveData CreateEditedItemSaveData();

        #endregion

        #region Pool

        public void RegisterDespawnCallback(Action action)
        {
            _backPoolAction += action;
        }

        public void UnregisterDespawnCallback(Action action)
        {
            _backPoolAction -= action;
        }

        public void InvokeDespawn()
        {
            _backPoolAction?.Invoke();
        }

        #endregion

        #region Ability

        #region Move

        public async UniTask MoveAsync(Vector2 coordinate, MoveDataSo moveDataSo)
        {
            SelfCoordinate = coordinate;
            DisposeMoveTokenSource();
            NewMoveTokenSource();

            await AbilityExtension.MoveHandle(transform, coordinate, moveDataSo, _moveCancellationTokenSource.Token);
        }

        private void NewMoveTokenSource()
        {
            _moveCancellationTokenSource = new CancellationTokenSource();
        }

        private void DisposeMoveTokenSource()
        {
            if (_moveCancellationTokenSource is not { IsCancellationRequested: false }) return;

            _moveCancellationTokenSource.Cancel();
            _moveCancellationTokenSource.Dispose();
            _moveCancellationTokenSource = null;
        }

        #endregion

        #endregion
    }
}