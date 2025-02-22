using System.Collections.Generic;
using _Game.Development.Interface.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Controller.Board
{
    public class BoardSaveController : MonoBehaviour
    {
        #region Unity Action

        private async void Start()
        {
            await UniTask.RunOnThreadPool(() =>
            {
                var dummy = 0;
                for (var i = 0; i < 1000; i++) dummy += i;
            });
        }

        #endregion

        public void TrySaveBoardData()
        {
            Save().Forget();
        }

        private UniTaskVoid Save()
        {
            //var json = await UniTask.RunOnThreadPool(() =>
            //{
                var itemSaveDataList = CreateItemSaveDataList();

                BoardExtension.Statics.BoardJsonData.itemSaveDataList = itemSaveDataList;
                var json= BoardExtension.Statics.BoardJsonData.ConvertToJson();
            //});

            BoardExtension.SaveBoardJson(json);
            return default;
        }

        private static List<ItemSaveData> CreateItemSaveDataList()
        {
            var itemSaveDataList = new List<ItemSaveData>();
            foreach (var gridData in BoardExtension.Statics.GridDataList)
            {
                var itemSaveData = gridData.GetComponent<IItem>().GetItemSaveData();
                itemSaveDataList.Add(itemSaveData);
            }

            return itemSaveDataList;
        }
    }
}