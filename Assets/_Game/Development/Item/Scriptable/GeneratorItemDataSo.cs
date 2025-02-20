using System;
using _Game.Development.Extension.Static;
using _Game.Development.Grid;
using UnityEngine;

namespace _Game.Development.Item.Scriptable
{
    [CreateAssetMenu(fileName = "GeneratorItemDataSo", menuName = "Game/Core/Board/Item/Generator/Data")]
    public class GeneratorItemDataSo : ItemDataSo
    {
        [Header("GeneratorItemData")] public GeneratorType generatorType;

        public int spawnAmount;

        [Tooltip("In seconds")] public int chargeDuration;

        [Space(5)] public PercentageData[] generateItemDataList;

        protected GeneratorItemDataSo()
        {
            itemType = ItemType.Generator;
            generateItemDataList = Array.Empty<PercentageData>();
        }

        public override int GetSpecialId()
        {
            return generatorType.ToInt();
        }
    }
}