using System.Collections.Generic;
using System.IO;
using _Game.Development.Factory.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Board;
using _Game.Development.Serializable.Factory;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using _Game.Development.Ui.Grid;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardEditController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GridEditorItemButton gridEditorItemButton;

        [SerializeField] private Transform parent;

        [Button]
        public void LoadBoardJson(TextAsset json)
        {
            if (json == null)
            {
                Debug.LogWarning("json Can not null!");
                return;
            }

            if (!Application.isPlaying)
            {
                Debug.LogWarning("Only Play Mode");
                return;
            }

            _boardJsonData = json.text.ConvertToBoardJsonData();
        }

        [Button]
        public void GenerateBoardJson(int rows, int columns)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Only Play Mode");
                return;
            }

            _boardJsonData = new BoardJsonData(rows, columns, new List<ItemSaveData>());

            var halfOfRows = _boardJsonData.Rows * 0.5f;
            var halfOfColumns = _boardJsonData.Columns * 0.5f;
            var offset = new Vector2(halfOfRows, halfOfColumns) - VectorExtension.Parameters.HalfSize;

            var itemDataSo = _allItemDataSo.GetEmptyItemDataSo();
            var itemId = itemDataSo.GetItemId();

            for (var x = 0; x < _boardJsonData.Rows; x++)
            for (var y = 0; y < _boardJsonData.Columns; y++)
            {
                var coordinate = new Vector2(x, y) - offset;
                var createDefaultItemSaveDataFunc = ItemFactory.CreateDefaultItemSaveDataByItemId[itemId];
                var defaultParameters = new DefaultSaveParameters(coordinate, itemDataSo);
                var newItemSaveData = createDefaultItemSaveDataFunc.Invoke(defaultParameters);

                _boardJsonData.ItemSaveDataList.Add(newItemSaveData);
            }
        }

        [Button]
        public void SaveBoardJson()
        {
            var json = new BoardJsonData(_boardJsonData.Rows, _boardJsonData.Columns, _boardJsonData.ItemSaveDataList)
                .ConvertToJson();
            File.WriteAllText(BoardExtension.Parameters.JsonPath, json);
            AssetDatabase.SaveAssets();
        }

        private void CreateUiElements()
        {
            foreach (var itemTypeData in _allItemDataSo.itemDataSoList)
            foreach (var specialIdData in itemTypeData.specialIdDataList)
            foreach (var itemDataSo in specialIdData.itemItemDataSoList)
            {
                var levelTileProperty = Instantiate(gridEditorItemButton, parent);
                levelTileProperty.Init(itemDataSo, this);
            }
        }

        #region Parameters

        [Inject] private AllItemDataSo _allItemDataSo;
        private BoardJsonData _boardJsonData;

        #endregion

        #region Getter Setter

        private ItemSaveData GetItemSaveDataByCoordinate(Vector3 coordinate, out int index)
        {
            for (var i = 0; i < _boardJsonData.ItemSaveDataList.Count; i++)
            {
                var itemSaveData = _boardJsonData.ItemSaveDataList[i];
                var bottomLeft = itemSaveData.coordinate.ToVector2() - VectorExtension.Parameters.HalfSize;
                var topRight = itemSaveData.coordinate.ToVector2() + VectorExtension.Parameters.HalfSize;

                var isDotIn = VectorExtension.CheckOverlapWithDot(bottomLeft, topRight, coordinate);
                if (!isDotIn) continue;

                index = i;
                return itemSaveData;
            }

            index = -1;
            return null;
        }

        private ItemSaveData GetItemSaveData(out int index)
        {
            var coordinate = _camera.GetCameraPosition();
            return GetItemSaveDataByCoordinate(coordinate, out index);
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public BoardJsonData GetBoardJsonData()
        {
            return _boardJsonData;
        }

        public void SetSelectedItemDataSo(ItemDataSo itemDataSo)
        {
            _selectedItemDataSo = itemDataSo;
        }

        public ItemDataSo GetSelectedGridItemDataSo()
        {
            return _selectedItemDataSo;
        }

        public ItemDataSo GetItemDataSoByItemSaveData(ItemSaveData itemSaveData)
        {
            return _allItemDataSo.GetItemDataSoByItemSaveData(itemSaveData);
        }

        #endregion

        #region Parameters

        private ItemDataSo _selectedItemDataSo;
        [Inject] private Camera _camera;

        #endregion

        #region Unity Action

        private void Start()
        {
            CreateUiElements();
            SetSelectedItemDataSo(_allItemDataSo.GetEmptyItemDataSo());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) HandleItemPlacement();
        }

        private void HandleItemPlacement()
        {
            var itemSaveData = GetItemSaveData(out var index);
            if (itemSaveData == null || index == -1) return;

            var itemDataSo = Input.GetMouseButtonDown(0)
                ? _selectedItemDataSo
                : _allItemDataSo.GetEmptyItemDataSo();

            var itemId = itemDataSo.GetItemId();
            var coordinate = itemSaveData.coordinate.ToVector2();
            var createDefaultItemSaveDataFunc = ItemFactory.CreateDefaultItemSaveDataByItemId[itemId];
            var defaultParameters = new DefaultSaveParameters(coordinate, itemDataSo);
            var newItemSaveData = createDefaultItemSaveDataFunc.Invoke(defaultParameters);

            _boardJsonData.ItemSaveDataList[index] = newItemSaveData;

            SaveBoardJson();
        }

        #endregion
    }
}