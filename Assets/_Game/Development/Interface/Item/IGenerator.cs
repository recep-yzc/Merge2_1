using _Game.Development.Scriptable.Item;

namespace _Game.Development.Interface.Item
{
    public interface IGenerator : IItem
    {
        public bool CanGenerate();
        public int GetSpawnAmount();
        public ItemDataSo Generate();
        
        public void FetchLastUsingDate(string date);
    }
}