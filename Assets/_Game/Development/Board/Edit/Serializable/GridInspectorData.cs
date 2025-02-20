using System;
using _Game.Development.Extension.Serializable;
using _Game.Development.Extension.Static;
using _Game.Development.Item.Scriptable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class GridInspectorData
    {
        public SerializableVector2 coordinate;
        public ItemDataSo itemDataSo;
    }
}