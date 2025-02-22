using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;

namespace _Game.Development.Object.Item
{
    public class Empty : Item, IEmpty
    {
        #region Item

        public override ItemSaveData CreateItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(SelfCoordinate);
            return new EmptyItemSaveData(gridData.coordinate.ToJsonVector2(), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId());
        }

        #endregion
    }
}