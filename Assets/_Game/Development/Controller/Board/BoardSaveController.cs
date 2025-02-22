using System.Collections.Generic;
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

        private async UniTask Save()
        {
            var json = await UniTask.RunOnThreadPool(() =>
            {
                var itemSaveDataList = CreateItemSaveDataList();

                BoardExtension.Statics.BoardJsonData.itemSaveDataList = itemSaveDataList;
                return BoardExtension.Statics.BoardJsonData.ConvertToJson();
            });

            BoardExtension.SaveBoardJson(json);
        }

        private static List<ItemSaveData> CreateItemSaveDataList()
        {
            var itemSaveDataList = new List<ItemSaveData>();
            foreach (var gridData in BoardExtension.Statics.GridDataList)
            {
                var itemDataSo = gridData.itemDataSo;
                itemSaveDataList.Add(new ItemSaveData(
                    new SerializableVector2(gridData.coordinate),
                    itemDataSo.level,
                    itemDataSo.itemType.ToInt(),
                    itemDataSo.GetSpecialId()));
            }

            return itemSaveDataList;
        }
    }
}