using System;
using _Game.Development.Item.Scriptable;
using _Game.Development.Item.Serializable;

namespace _Game.Development.Board.Edit.Serializable
{
    [Serializable]
    public class GridInspectorData
    {
        public SerializableVector2 coordinate;
        public ItemDataSo itemDataSo;
    }
}