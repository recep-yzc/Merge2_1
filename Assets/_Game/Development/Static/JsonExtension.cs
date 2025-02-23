using _Game.Development.Serializable.Board;
using Newtonsoft.Json;
using UnityEngine;

namespace _Game.Development.Static
{
    public static class JsonExtension
    {
        public static string ConvertToJson(this BoardJsonData boardJsonData)
        {
            return JsonConvert.SerializeObject(boardJsonData, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
        }

        public static BoardJsonData ConvertToBoardJsonData(this string json)
        {
            return JsonConvert.DeserializeObject<BoardJsonData>(json,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
        }

        public static BoardJsonData ConvertToBoardJsonData(this TextAsset json)
        {
            return JsonConvert.DeserializeObject<BoardJsonData>(json.text,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });
        }
    }
}