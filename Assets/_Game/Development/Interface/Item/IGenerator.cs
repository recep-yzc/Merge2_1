using _Game.Development.Scriptable.Item;

namespace _Game.Development.Interface.Item
{
    public interface IGenerator : IItem
    {
        public int GetSpawnAmount();
        public ItemDataSo Generate();
    }
}