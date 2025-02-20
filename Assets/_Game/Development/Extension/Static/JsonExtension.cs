using _Game.Development.Board.Edit.Serializable;
using Newtonsoft.Json;
using UnityEngine;

namespace _Game.Development.Extension.Static
{
    public static class JsonExtension
    {
        public static BoardJsonData ConvertToBoardJsonData(this TextAsset json)
        {
            return JsonConvert.DeserializeObject<BoardJsonData>(json.text, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
        
        public static BoardJsonData ConvertToBoardJsonData(this string json)
        {
            return JsonConvert.DeserializeObject<BoardJsonData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
        }
    }
}