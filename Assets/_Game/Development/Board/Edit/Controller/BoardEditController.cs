using System.Collections.Generic;
using System.IO;
using System.Linq;
using _Game.Development.Board.Edit.Scriptable;
using _Game.Development.Board.Edit.Serializable;
using _Game.Development.Board.Edit.Ui;
using _Game.Development.Extension.Static;
using _Game.Development.Item;
using _Game.Development.Item.Scriptable;
using _Game.Development.Item.Serializable;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace _Game.Development.Board.Edit.Controller
{
    public class BoardEditController : MonoBehaviour
    {
        #region Parameters

        private string JsonPath => Application.dataPath + "/Board.json";

        #endregion

        [Header("References")] [SerializeField]
        private AllItemDataSo allItemDataSo;

        [SerializeField] private BoardDataSo boardDataSo;

        [SerializeField] private GridEditorItemButton gridEditorItemButton;
        [SerializeField] private Transform parent;

        [Button("Load Board Json")]
        public void LoadBoardJson(TextAsset boardJson)
        {
            var boardJsonData = JsonConvert.DeserializeObject<BoardJsonData>(boardJson.text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            boardDataSo.rows = boardJsonData.rows;
            boardDataSo.columns = boardJsonData.columns;
            boardDataSo.gridInspectorDataList.Clear();

            foreach (var gridJsonData in boardJsonData.gridJsonDataList)
            {
                var itemTypeData = allItemDataSo.itemDataSoList.First(x => x.itemType.ToInt() == gridJsonData.itemId);
                var specialIdData = itemTypeData.specialIdDataList.First(x => x.specialId == gridJsonData.specialId);
                var itemDataSo = specialIdData.itemItemDataSoList.First(x => x.level == gridJsonData.level);

                boardDataSo.gridInspectorDataList.Add(new GridInspectorData
                {
                    coordinate = gridJsonData.coordinate,
                    itemDataSo = itemDataSo
                });
            }
        }

        [Button]
        public void GenerateGridData()
        {
            boardDataSo.gridInspectorDataList.Clear();

            var halfOfRows = boardDataSo.rows * 0.5f;
            var halfOfColumns = boardDataSo.columns * 0.5f;
            var offset = new Vector2(halfOfRows, halfOfColumns) - VectorExtension.HalfSize;

            GridInspectorData CreateGridData(int x, int y)
            {
                return new GridInspectorData
                {
                    coordinate = new SerializableVector2(new Vector2(x, y) - offset),
                    itemDataSo = allItemDataSo.GetEmptyItemDataSo()
                };
            }

            for (var x = 0; x < boardDataSo.rows; x++)
            for (var y = 0; y < boardDataSo.columns; y++)
                boardDataSo.gridInspectorDataList.Add(CreateGridData(x, y));
        }

        [Button("Generate Board Json")]
        public void GenerateBoardJson()
        {
            List<ItemSaveData> gridJsonData = new();

            foreach (var gridData in boardDataSo.gridInspectorDataList)
            {
                var specialId = gridData.itemDataSo.GetSpecialId();

                var func = ItemFactory.CreateItemSaveDataBySpecialId[specialId];
                var itemSaveData = func.Invoke(gridData);

                gridJsonData.Add(itemSaveData);
            }

            var json = JsonConvert.SerializeObject(
                new BoardJsonData(boardDataSo.rows, boardDataSo.columns, gridJsonData), Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

            File.WriteAllText(JsonPath, json);
            AssetDatabase.SaveAssets();
        }

        private void FetchGridNeighborList()
        {
            /* BoardGlobalValues.GridDataList = gridDataList;

             foreach (var gridData in gridDataList)
             {
                 List<GridData> neighborList = new();

                 foreach (var directionId in EnumExtension.ToArray<DirectionId>())
                 {
                     var direction = directionId.DirectionToVector();
                     var coordinate = gridData.coordinate + direction;
                     var neighborGridData = BoardExtension.GetGridDataByCoordinate(coordinate);

                     if (neighborGridData == null) continue;
                     neighborList.Add(neighborGridData);
                 }

                 gridData.SetNeighborGridData(neighborList.ToList());
             }*/
        }

        private void ChangeGridItemDataSo(GridInspectorData gridInspectorData, ItemDataSo itemDataSo)
        {
            gridInspectorData.itemDataSo = itemDataSo;
        }

        private void CreateUiElements()
        {
            foreach (var itemTypeData in allItemDataSo.itemDataSoList)
            foreach (var specialIdData in itemTypeData.specialIdDataList)
            foreach (var itemDataSo in specialIdData.itemItemDataSoList)
            {
                var levelTileProperty = Instantiate(gridEditorItemButton, parent);
                levelTileProperty.Init(itemDataSo, this);
            }
        }

        #region Getter Setter

        private GridInspectorData GetGridInspectorDataByCoordinate(Vector3 coordinate)
        {
            foreach (var gridData in boardDataSo.gridInspectorDataList)
            {
                var bottomLeft = gridData.coordinate.ToVector2() - VectorExtension.HalfSize;
                var topRight = gridData.coordinate.ToVector2() + VectorExtension.HalfSize;

                var isDotIn = VectorExtension.CheckOverlapWithDot(bottomLeft, topRight, coordinate);
                if (!isDotIn) continue;

                return gridData;
            }

            return null;
        }

        private GridInspectorData GetGridInspectorData()
        {
            var coordinate = _camera.GetCoordinate();
            var gridInspectorData = GetGridInspectorDataByCoordinate(coordinate);
            return gridInspectorData;
        }

        public Camera GetCamera()
        {
            return _camera;
        }

        public BoardDataSo GetLevelDataSo()
        {
            return boardDataSo;
        }

        public void SetSelectedItemDataSo(ItemDataSo itemDataSo)
        {
            _selectedItemDataSo = itemDataSo;
        }

        public ItemDataSo GetSelectedGridItemDataSo()
        {
            return _selectedItemDataSo;
        }

        #endregion

        #region Parameters

        private ItemDataSo _selectedItemDataSo;
        private Camera _camera;

        #endregion

        #region Unity Action

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            CreateUiElements();
            SetSelectedItemDataSo(allItemDataSo.GetEmptyItemDataSo());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var gridInspectorData = GetGridInspectorData();
                if (gridInspectorData == null) return;

                ChangeGridItemDataSo(gridInspectorData, _selectedItemDataSo);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var gridInspectorData = GetGridInspectorData();
                if (gridInspectorData == null) return;

                ChangeGridItemDataSo(gridInspectorData, allItemDataSo.GetEmptyItemDataSo());
            }
        }

        #endregion
    }
}