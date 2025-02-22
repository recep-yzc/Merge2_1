using System;
using _Game.Development.Enum.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Scriptable.Item
{
    [CreateAssetMenu(fileName = "GeneratorItemDataSo", menuName = "Game/Core/Board/Item/Generator/Data")]
    public class GeneratorItemDataSo : ItemDataSo
    {
        [Header("GeneratorItemData")] public GeneratorType generatorType;

        public int spawnAmount;

        [Tooltip("In seconds")] public int chargeDuration;
        [Space(5)] public PercentageData[] spawnableItemDataList;

        protected GeneratorItemDataSo()
        {
            itemType = ItemType.Generator;
            spawnableItemDataList = Array.Empty<PercentageData>();
        }

        public override int GetSpecialId()
        {
            return generatorType.ToInt();
        }
    }
}