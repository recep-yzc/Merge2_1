using System;
using UnityEngine;

namespace _Game.Development.Item
{
    public abstract class Item : MonoBehaviour, IItem
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        #region Parameters

        private Action _backPoolAction;

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


        public void SetSpriteOrder(int order)
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