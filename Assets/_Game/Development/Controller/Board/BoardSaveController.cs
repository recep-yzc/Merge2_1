using System.Collections.Generic;
using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Board;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Controller.Board
{
    public class BoardSaveController : MonoBehaviour
    {
        private void Save()
        {
            if (!BoardExtension.Parameters.IsBoardInitialized) return;

            var itemSaveDataList = CreateItemSaveDataList();

            var rows = BoardExtension.Parameters.BoardJsonData.Rows;
            var columns = BoardExtension.Parameters.BoardJsonData.Columns;
            var newBoardJsonData = new BoardJsonData(rows, columns, itemSaveDataList);

            var json = newBoardJsonData.ConvertToJson();
            BoardExtension.Parameters.BoardJsonData = newBoardJsonData;

            BoardExtension.SaveBoardJson(json);
        }

        private List<ItemSaveData> CreateItemSaveDataList()
        {
            var itemSaveDataList = new List<ItemSaveData>();
            foreach (var gridData in BoardExtension.Parameters.GridDataList)
            {
                var itemSaveData = gridData.GetComponent<IItem>().CreateEditedItemSaveData();
                itemSaveDataList.Add(itemSaveData);
            }

            return itemSaveDataList;
        }

        #region Unity Action

        private void OnApplicationQuit()
        {
            Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
        }

        #endregion
    }
}