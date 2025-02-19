using _Game.Development.Extension;
using _Game.Development.Item;
using _Game.Development.Level;
using UnityEngine;
using Zenject;

namespace _Game.Development.Board
{
    public class BoardLoadController : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private BoardSaveData boardSaveData;

        #region Parameters

        [Inject] private LevelDataSo _levelDataSo;

        #endregion

        private BoardSaveData InitBoardSaveData()
        {
            var tempBoardSaveData = new BoardSaveData();

            foreach (var levelGridData in _levelDataSo.levelGridDataList)
            {
                var type = levelGridData.itemDataSo.itemType.ToInt();
                var itemSaveData = ItemFactory.CreateItemSaveDataByCategoryType[type].Invoke(levelGridData);
                tempBoardSaveData.ItemSaveDataList.Add(itemSaveData);
            }

            return tempBoardSaveData;
        }

        public BoardSaveData Load()
        {
            var isFirstLoad = !PlayerPrefs.HasKey("BoardSaveData");
            boardSaveData = isFirstLoad ? InitBoardSaveData() : BoardSaveController.BoardSaveDataPlayerPrefs;
            return boardSaveData;
        }
    }
}