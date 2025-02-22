using System.Linq;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Controller.Board
{
    public class BoardGenerateController : MonoBehaviour
    {
        public void TryGenerate(GridData mouseDownGridData)
        {
            var generator = mouseDownGridData.item.GetComponent<IGenerator>();
            if (generator is null) return;

            var selectedGridData = BoardExtension.Statics.GridDataList.FirstOrDefault(x => x.IsEmpty());
            if (selectedGridData == null)
            {
                Debug.Log("Board Full");
                return;
            }

            var spawnAmount = generator.GetSpawnAmount();
            if (spawnAmount <= 0) return;

            var itemDataSo = generator.Generate();

            var itemSaveData = ItemFactory.CreateItemSaveDataByItemId[itemDataSo.itemType.ToInt()]
                .Invoke(selectedGridData.coordinate, itemDataSo);
            var item = ItemFactory.CreateItemByItemId[itemDataSo.itemType.ToInt()].Invoke(itemSaveData);

            selectedGridData.item = item;
            selectedGridData.itemDataSo = itemDataSo;
        }
    }
}