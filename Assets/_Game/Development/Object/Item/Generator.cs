using System;
using System.Linq;
using System.Threading;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Development.Object.Item
{
    public class Generator : Item, IGenerator, IDraggable, IClickable
    {
        [Header("References")] [SerializeField]
        private GameObject regenerateCanvas;

        [SerializeField] private Image imgRegenerate;

        #region Unity Action

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeRegenerateToken();
        }

        #endregion

        #region Draggable

        public void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * DragMoveSpeed);
        }

        #endregion

        #region Parameters

        private int _spawnAmount;
        private string _lastUsingDate;

        private CancellationTokenSource _regenerateCancellationTokenSource;

        #endregion

        #region Item

        public override ItemSaveData GetItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(Position);
            return new GeneratorItemSaveData(gridData.coordinate.ToJsonVector2(), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId(), _lastUsingDate);
        }

        public override void FetchItemData()
        {
            RefillSpawnAmount();
        }

        #endregion

        #region Generator

        public void FetchLastUsingDate(string date)
        {
            _lastUsingDate = date;
        }

        public int GetSpawnAmount()
        {
            return _spawnAmount;
        }

        private void RefillSpawnAmount()
        {
            _spawnAmount = ((GeneratorItemDataSo)ItemDataSo).spawnAmount;
        }

        public ItemDataSo Generate()
        {
            _spawnAmount--;
            if (_spawnAmount <= 0) StartRegenerate().Forget();

            _lastUsingDate = DateTime.Now.ToString(CultureExtension.CurrentCultureInfo);

            var generateItemDataList = ((GeneratorItemDataSo)ItemDataSo).generateItemDataList;
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

            await AbilityExtension.Regenerating(((GeneratorItemDataSo)ItemDataSo).chargeDuration, imgRegenerate,
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

        public void MouseDown()
        {
            SetSpriteOrder(1);
        }

        public void MouseUp()
        {
            SetSpriteOrder(0);
        }

        #endregion
    }
}