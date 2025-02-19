using System.Collections.Generic;
using _Game.Development.Extension;
using _Game.Development.Item;
using UnityEngine;
using Zenject;

namespace _Game.Development.Level
{
    public class LevelEditor : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private GridEditorItemButton gridEditorItemButton;

        [SerializeField] private Transform parent;
        [SerializeField] private List<ItemDataSo> gridItemDataSoList;

        private void UpdateGridItemDataSo(GridData gridData, ItemDataSo itemDataSo)
        {
            gridData.itemDataSo = itemDataSo;
        }

        private GridData GetGridDataSoByCoordinate(Vector3 coordinate)
        {
            foreach (var gridData in _levelDataSo.gridDataList)
            {
                var bottomLeft = gridData.coordinate - VectorExtension.HalfSize;
                var topRight = gridData.coordinate + VectorExtension.HalfSize;

                var isDotIn = VectorExtension.CheckOverlapWithDot(bottomLeft, topRight, coordinate);
                if (!isDotIn) continue;

                return gridData;
            }

            return null;
        }

        private GridData GetGridData()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = _camera.transform.position.y;

            var inputPosition = _camera.ScreenToWorldPoint(mousePosition);
            var gridData = GetGridDataSoByCoordinate(inputPosition);
            return gridData;
        }

        #region Parameters

        [Inject] private LevelDataSo _levelDataSo;
        public static ItemDataSo SelectedItemDataSo;
        private Camera _camera;

        #endregion

        #region Unity Action

        private void Start()
        {
            _camera = Camera.main;

            foreach (var tilePropertyDataSo in gridItemDataSoList)
            {
                var levelTileProperty = Instantiate(gridEditorItemButton, parent);
                levelTileProperty.Init(tilePropertyDataSo);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var gridData = GetGridData();
                if (gridData == null) return;

                UpdateGridItemDataSo(gridData, SelectedItemDataSo);
                _levelDataSo.Save();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var gridData = GetGridData();
                if (gridData == null) return;

                UpdateGridItemDataSo(gridData, null);
                _levelDataSo.Save();
            }
        }

        #endregion
    }
}