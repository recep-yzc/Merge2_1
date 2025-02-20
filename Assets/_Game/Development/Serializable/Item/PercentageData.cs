using System;
using _Game.Development.Scriptable.Item;
using Sirenix.OdinInspector;

namespace _Game.Development.Serializable.Item
{
    [Serializable]
    public class PercentageData
    {
        [MinValue(0)] [MaxValue(100)] public float percentage;
        public ItemDataSo itemDataSo;
    }
}