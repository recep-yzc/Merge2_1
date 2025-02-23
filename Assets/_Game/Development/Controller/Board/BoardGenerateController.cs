using System.Linq;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Factory;
using _Game.Development.Serializable.Grid;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace _Game.Development.Controller.Board
{
    public class BoardGenerateController : MonoBehaviour
    {
        public void TryGenerate(GridData mouseDownGridData)
        {
            var generator = mouseDownGridData.item.GetComponent<IGenerator>();
            if (generator is null) return;

            var canGenerate = generator.CanGenerate();
            if (!canGenerate) return;

            var spawnAmount = generator.GetSpawnCount();
            if (spawnAmount <= 0)
            {
                Debug.Log("Charging!");
                return;
            }

            var emptyGridData = BoardExtension.Parameters.GridDataList.Where(x => x.IsEmpty()).ToList();
            if (!emptyGridData.Any())
            {
                Debug.Log("Board Full!");
                return;
            }

            var random = new Random();
            var selectedGridData = emptyGridData[random.Next(emptyGridData.Count)];
            if (selectedGridData == null) return;

            mouseDownGridData.item.GetComponent<IScaleUpDown>()?.ScaleUpDownAsync(_scaleUpDownDataSo).Forget();

            var generatorCoordinate = mouseDownGridData.Coordinate;
            Create(generator, generatorCoordinate, selectedGridData);
        }

        private void Create(IGenerator generator, Vector2 generatorCoordinate, GridData gridData)
        {
            gridData.GetComponent<IPool>().InvokeDespawn();

            var itemDataSo = generator.Generate();
            var itemId = itemDataSo.GetItemId();
            
            var coordinate = gridData.Coordinate;
            var func = ItemFactory.CreateDefaultItemSaveDataByItemId[itemId];
            var defaultParameters = new DefaultSaveParameters(coordinate, itemDataSo);
            var itemSaveData = func.Invoke(defaultParameters);
            
            var item = ItemFactory.CreateItemByItemId[itemId].Invoke(itemSaveData);

            item.GetComponent<IItem>().SetPosition(generatorCoordinate);
            item.GetComponent<IMoveable>().MoveAsync(gridData.Coordinate, _moveDataSo).Forget();

            gridData.item = item;
            gridData.itemDataSo = itemDataSo;
        }

        #region Parameters

        [Inject] private MoveDataSo _moveDataSo;
        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        #endregion
    }
}