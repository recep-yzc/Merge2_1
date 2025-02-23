using System;

namespace _Game.Development.Interface.Property
{
    public interface IPool
    {
        public void InvokeDespawn();
        public void RegisterDespawnCallback(Action action);
        public void UnregisterDespawnCallback(Action action);
    }
}