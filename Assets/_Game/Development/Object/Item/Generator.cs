using System;
using System.Linq;
using System.Threading;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Grid;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Development.Object.Item
{
    public class Generator : Item, IGenerator
    {
        [Header("References")] [SerializeField]
        private GameObject regenerateCanvas;

        [SerializeField] private Image imgRegenerate;

        #region Draggable

        public override void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Parameters

        private int _spawnAmount;
        private GeneratorItemDataSo _generatorItemDataSo;
        private string _lastUsingDate;

        private CancellationTokenSource _regenerateCancellationTokenSource;

        #endregion


        #region Unity Action

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeRegenerateToken();
        }

        #endregion

        #region Item

        public override void FetchCustomParameters(params object[] parameters)
        {
            base.FetchCustomParameters(parameters);
            _lastUsingDate = parameters[0].ToString();
        }

        public override ItemSaveData GetItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(transform.position);
            return new GeneratorItemSaveData(new SerializableVector2(gridData.coordinate), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId(), _lastUsingDate);
        }

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            base.SetItemDataSo(itemDataSo);
            _generatorItemDataSo = (GeneratorItemDataSo)itemDataSo;
        }

        public override void FetchItemData()
        {
            RefillSpawnAmount();
        }

        #endregion

        #region Generator

        public int GetSpawnAmount()
        {
            return _spawnAmount;
        }

        private void RefillSpawnAmount()
        {
            _spawnAmount = _generatorItemDataSo.spawnAmount;
        }

        public ItemDataSo Generate()
        {
            _spawnAmount--;
            if (_spawnAmount <= 0)
            {
                StartRegenerate().Forget();
            }

            _lastUsingDate = DateTime.Now.ToString(CultureExtension.CurrentCultureInfo);

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

        private async UniTask StartRegenerate()
        {
            regenerateCanvas.SetActive(true);

            DisposeRegenerateToken();
            _regenerateCancellationTokenSource = new CancellationTokenSource();

            await AbilityExtension.Regenerating(_generatorItemDataSo.chargeDuration, imgRegenerate,
                _regenerateCancellationTokenSource.Token);

            regenerateCanvas.SetActive(false);
            RefillSpawnAmount();
        }

        private void DisposeRegenerateToken()
        {
            _regenerateCancellationTokenSource?.Cancel();
            _regenerateCancellationTokenSource?.Dispose();
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