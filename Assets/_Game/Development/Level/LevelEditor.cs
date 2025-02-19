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

        private void UpdateGridItemDataSo(LevelGridData levelGridData, ItemDataSo itemDataSo)
        {
            levelGridData.itemDataSo = itemDataSo;
        }

        private LevelGridData GetLevelGridDataSoByCoordinate(Vector3 coordinate)
        {
            foreach (var levelGridData in _levelDataSo.levelGridDataList)
            {
                var bottomLeft = levelGridData.coordinate - VectorExtension.HalfSize;
                var topRight = levelGridData.coordinate + VectorExtension.HalfSize;

                var isDotIn = VectorExtension.CheckOverlapWithDot(bottomLeft, topRight, coordinate);
                if (!isDotIn) continue;

                return levelGridData;
            }

            return null;
        }

        private LevelGridData GetLevelGridData()
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = _camera.transform.position.y;

            var inputPosition = _camera.ScreenToWorldPoint(mousePosition);
            var levelGridData = GetLevelGridDataSoByCoordinate(inputPosition);
            return levelGridData;
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
                var levelGridData = GetLevelGridData();
                if (levelGridData == null) return;

                UpdateGridItemDataSo(levelGridData, SelectedItemDataSo);
                _levelDataSo.Save();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                var levelGridData = GetLevelGridData();
                if (levelGridData == null) return;

                UpdateGridItemDataSo(levelGridData, null);
                _levelDataSo.Save();
            }
        }

        #endregion
    }
}