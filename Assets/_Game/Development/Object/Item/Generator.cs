using System.Linq;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using UnityEngine;

namespace _Game.Development.Object.Item
{
    public class Generator : Item, IGenerator
    {
        #region Draggable

        public override void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Parameters

        private int _spawnAmount;
        private GeneratorItemDataSo _generatorItemDataSo;

        #endregion

        #region Item

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            base.SetItemDataSo(itemDataSo);
            _generatorItemDataSo = (GeneratorItemDataSo)itemDataSo;
        }

        public override void FetchItemData()
        {
            _spawnAmount = _generatorItemDataSo.spawnAmount;
        }

        #endregion


        #region Generator

        public int GetSpawnAmount()
        {
            return _spawnAmount;
        }

        public ItemDataSo Generate()
        {
            var generateItemDataList = _generatorItemDataSo.generateItemDataList;
            var percentage = generateItemDataList.Sum(x => x.percentage);

            var randomValue = Random.Range(0f, percentage);
            var cumulative = 0f;

            foreach (var data in generateItemDataList)
            {
                cumulative += data.percentage;
                if (randomValue <= cumulative) return data.itemDataSo;
            }

            return null;
        }

        #endregion

        #region Clickable

        public override void MouseDown()
        {
            SetSpriteOrder(1);
        }

        public override void MouseUp()
        {
            SetSpriteOrder(0);
        }

        #endregion
    }
}