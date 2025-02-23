using System.Linq;
using _Game.Development.Enum.Board;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Item;
using _Game.Development.Interface.Property;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Factory;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace _Game.Development.Controller.Board
{
    public class BoardGenerateController : MonoBehaviour
    {
        public void TryGenerate(GridData sourceGridData)
        {
            var generator = GetGenerator(sourceGridData);
            if (generator == null || !CanGenerate(generator)) return;

            var spawnCount = GetSpawnCount(generator);
            if (spawnCount <= 0)
            {
                Debug.Log("Charging!");
                return;
            }

            var targetGridData = GetRandomEmptyGridData();
            if (targetGridData is null)
            {
                Debug.Log("Board Full!");
                return;
            }

            ScaleUpDown(sourceGridData);
            CreateNewItem(generator, sourceGridData.Coordinate, targetGridData);
        }

        private IGenerator GetGenerator(GridData gridData)
        {
            return gridData.item.GetComponent<IGenerator>();
        }

        private bool CanGenerate(IGenerator generator)
        {
            return generator.CanGenerate();
        }

        private int GetSpawnCount(IGenerator generator)
        {
            return generator.GetSpawnCount();
        }

        private GridData GetRandomEmptyGridData()
        {
            var emptyGridData = BoardExtension.Parameters.GridDataList.Where(x => x.IsEmpty()).ToList();
            if (!emptyGridData.Any()) return null;

            var random = new Random();
            return emptyGridData[random.Next(emptyGridData.Count)];
        }

        private void ScaleUpDown(GridData gridData)
        {
            _boardScaleUpDownController.TryScaleUpDown(gridData);
        }

        private void CreateNewItem(IGenerator generator, Vector2 generatorCoordinate, GridData targetGridData)
        {
            DespawnExistingItem(targetGridData);

            var itemDataSo = GenerateItemData(generator);
            var itemId = itemDataSo.GetItemId();
            var coordinate = targetGridData.Coordinate;

            var itemSaveData = GenerateItemSaveData(itemId, coordinate, itemDataSo);
            var item = InstantiateItem(itemId, itemSaveData);

            UpdateGridData(targetGridData, item, itemDataSo);

            SetItemPosition(item, generatorCoordinate);
            MoveItemAsync(targetGridData, coordinate);
        }

        private void DespawnExistingItem(GridData gridData)
        {
            gridData.GetComponent<IPool>().InvokeDespawn();
        }

        private ItemDataSo GenerateItemData(IGenerator generator)
        {
            return generator.Generate();
        }

        private ItemSaveData GenerateItemSaveData(int itemId, Vector2 coordinate, ItemDataSo itemDataSo)
        {
            var defaultParameters = new DefaultSaveParameters(coordinate, itemDataSo);
            return ItemFactory.CreateDefaultItemSaveDataByItemId[itemId].Invoke(defaultParameters);
        }

        private GameObject InstantiateItem(int itemId, ItemSaveData itemSaveData)
        {
            return ItemFactory.CreateItemByItemId[itemId].Invoke(itemSaveData);
        }

        private void SetItemPosition(GameObject itemGameObject, Vector2 coordinate)
        {
            itemGameObject.GetComponent<IItem>().SetPosition(coordinate);
        }

        private void MoveItemAsync(GridData gridData, Vector2 coordinate)
        {
            _boardTransferController.TryTransfer(TransferAction.SpecificMove, gridData, coordinate);
        }

        private void UpdateGridData(GridData gridData, GameObject item, ItemDataSo itemDataSo)
        {
            gridData.item = item;
            gridData.itemDataSo = itemDataSo;
        }

        #region Parameters

        [Inject] private ScaleUpDownDataSo _scaleUpDownDataSo;

        [Inject] private BoardScaleUpDownController _boardScaleUpDownController;
        [Inject] private BoardTransferController _boardTransferController;

        #endregion
    }
}