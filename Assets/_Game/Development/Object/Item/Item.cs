using System;
using _Game.Development.Interface.Item;
using DG.Tweening;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public abstract class Item : MonoBehaviour, IItem
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        #region Parameters

        private Action _backPoolAction;
        protected readonly float DragMoveSpeed = 15f;

        #endregion

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

        public void ScaleUpDown()
        {
            transform.DOScale(1.1f, 0.2f).SetLoops(2, LoopType.Yoyo).From(1).SetEase(Ease.Unset);
        }

        protected void SetSpriteOrder(int order)
        {
            sprIcon.sortingOrder = order;
        }

        #region Pool

        public Action GetBackPool()
        {
            return _backPoolAction;
        }

        public void AddBackPool(Action action)
        {
            _backPoolAction += action;
        }

        public void RemoveBackPool(Action action)
        {
            _backPoolAction -= action;
        }

        public void PlayBackPool()
        {
            _backPoolAction?.Invoke();
        }

        #endregion
    }
}