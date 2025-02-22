using System.Collections.Generic;
using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Controller.Board
{
    public class BoardSaveController : MonoBehaviour
    {
        private void Save()
        {
            var itemSaveDataList = CreateItemSaveDataList();

            BoardExtension.Statics.BoardJsonData.itemSaveDataList = itemSaveDataList;
            var json = BoardExtension.Statics.BoardJsonData.ConvertToJson();

            BoardExtension.SaveBoardJson(json);
        }

        private List<ItemSaveData> CreateItemSaveDataList()
        {
            var itemSaveDataList = new List<ItemSaveData>();
            foreach (var gridData in BoardExtension.Statics.GridDataList)
            {
                var itemSaveData = gridData.GetComponent<IItem>().CreateItemSaveData();
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