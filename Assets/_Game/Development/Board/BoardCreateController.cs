using _Game.Development.Item;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Board
{
    public class BoardCreateController : MonoBehaviour
    {
        public async UniTask Create(BoardSaveData boardSaveData)
        {
            foreach (var itemSaveData in boardSaveData.ItemSaveDataList)
            {
                var isExist = ItemFactory.CreateItemByCategoryType.TryGetValue(itemSaveData.itemType, out var func);
                if (isExist) func.Invoke(itemSaveData);
            }

            await UniTask.DelayFrame(1);
        }
    }
}