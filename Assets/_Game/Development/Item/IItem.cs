using System;
using UnityEngine;

namespace _Game.Development.Item
{
    public interface IItem
    {
        public void SetParent(Transform parent);
        public void SetPosition(Vector2 position);

        public void SetSprite(Sprite icon);

        #region Pool

        public void PlayBackPool();
        public void AddBackPool(Action action);
        public void RemoveBackPool(Action action);
        
        public Action GetBackPool();


        #endregion
   
    }
}