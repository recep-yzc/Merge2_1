using System;

namespace _Game.Development.Interface.Item
{
    public interface IPool
    {
        public void PlayDespawnPool();
        public void AddDespawnPool(Action action);
        public void RemoveDespawnPool(Action action);
    }
}