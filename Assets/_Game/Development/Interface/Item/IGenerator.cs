using _Game.Development.Scriptable.Item;

namespace _Game.Development.Interface.Item
{
    public interface IGenerator : IItem
    {
        public bool CanGenerate();
        public int GetSpawnCount();
        public ItemDataSo Generate();

        public void SetLastUsingDate(string date);
    }
}