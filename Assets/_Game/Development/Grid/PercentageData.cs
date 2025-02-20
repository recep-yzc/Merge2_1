using System;
using _Game.Development.Item.Scriptable;
using Sirenix.OdinInspector;

namespace _Game.Development.Grid
{
    [Serializable]
    public class PercentageData
    {
        [MinValue(0)] [MaxValue(100)] public float percentage;
        public ItemDataSo itemDataSo;
    }
}