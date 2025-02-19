using UnityEngine;

namespace _Game.Development.Board
{
    public class BoardSaveController : MonoBehaviour
    {
        public static BoardSaveData BoardSaveDataPlayerPrefs
        {
            get => JsonUtility.FromJson<BoardSaveData>(PlayerPrefs.GetString("BoardSaveData"));
            set => PlayerPrefs.SetString("BoardSaveData", JsonUtility.ToJson(value));
        }
    }
}