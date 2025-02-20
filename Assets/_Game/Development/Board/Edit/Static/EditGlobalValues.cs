using UnityEngine;

namespace _Game.Development.Board.Edit.Static
{
    public static class EditGlobalValues
    {
        //public static string JsonPath => Application.persistentDataPath + "/Board.json"; // Mobilde bu yol kullanılacak
        public static string JsonPath => Application.dataPath + "/Board.json"; // Editorde bu yol kullanılacak
    }
}