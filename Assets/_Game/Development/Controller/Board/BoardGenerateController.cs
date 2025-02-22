using System.Linq;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardGenerateController : MonoBehaviour
    {
        #region Parameters

        [Inject] private MoveDataSo _moveDataSo;

        #endregion

        public void TryGenerate(GridData mouseDownGridData)
        {
            var generator = mouseDownGridData.item.GetComponent<IGenerator>();
            if (generator is null) return;

            var canGenerate = generator.CanGenerate();
            if (!canGenerate) return;

            var spawnAmount = generator.GetSpawnAmount();
            if (spawnAmount <= 0)
            {
                Debug.Log("Charging!");
                return;
            }

            var emptyGridData = BoardExtension.Statics.GridDataList.Where(x => x.IsEmpty()).ToList();
            if (!emptyGridData.Any())
            {
                Debug.Log("Board Full!");
                return;
            }
            
            var random = new System.Random();
            var selectedGridData = emptyGridData[random.Next(emptyGridData.Count)];
            if (selectedGridData == null)
            {
                return;
            }

            var generatorCoordinate = mouseDownGridData.coordinate;
            Create(generator, generatorCoordinate, selectedGridData);
        }

        private void Create(IGenerator generator, Vector2 generatorCoordinate, GridData gridData)
        {
            var itemDataSo = generator.Generate();
            var itemId = itemDataSo.itemType.ToInt();

            var itemSaveData = ItemFactory.CreateItemSaveDataByItemId[itemId].Invoke(gridData.coordinate, itemDataSo);
            var item = ItemFactory.CreateItemByItemId[itemId].Invoke(itemSaveData);

            item.GetComponent<IItem>().SetPosition(generatorCoordinate);
            item.GetComponent<IMoveable>().MoveAsync(gridData.coordinate, _moveDataSo).Forget();

            gridData.item = item;
            gridData.itemDataSo = itemDataSo;
        }
    }
}