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
        private GeneratorItemDataSo _generatorItemDataSo;
        private CancellationTokenSource _regenerateCancellationTokenSource;

        #endregion

        #region Item

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            base.SetItemDataSo(itemDataSo);
            _generatorItemDataSo = (GeneratorItemDataSo)itemDataSo;
        }

        public override ItemSaveData CreateItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(SelfCoordinate);
            return new GeneratorItemSaveData(gridData.coordinate.ToJsonVector2(), gridData.itemDataSo.level,
                gridData.itemDataSo.itemType.ToInt(), gridData.itemDataSo.GetSpecialId(), _lastUsingDate);
        }

        public override void FetchItemData()
        {
            var lastUsingData = _lastUsingDate.StringToDateTime();
            var totalSeconds = (DateTime.Now - lastUsingData).TotalSeconds;
            var leftDuration = totalSeconds - _generatorItemDataSo.chargeDuration;
            if (leftDuration > 0)
                RefillSpawnAmount();
            else
                StartRegenerate(Mathf.Abs((float)leftDuration)).Forget();
        }

        #endregion

        #region Generator

        public override void LevelUp()
        {
            StopRegenerate();
            ResetLastUsingDate();
            RefillSpawnAmount();
        }

        private void ResetLastUsingDate()
        {
            _lastUsingDate = DateTime.Now.AddSeconds(_generatorItemDataSo.chargeDuration).DateTimeToString();
        }

        public void FetchLastUsingDate(string date)
        {
            _lastUsingDate = date;
        }

        public bool CanGenerate()
        {
            return _generatorItemDataSo.spawnableItemDataList.Length > 0;
        }

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
                var duration = _generatorItemDataSo.chargeDuration;
                StartRegenerate(duration).Forget();
            }

            _lastUsingDate = DateTime.Now.DateTimeToString();

            return GetRandomGeneratedItemDataSo();
        }

        private ItemDataSo GetRandomGeneratedItemDataSo()
        {
            var percentageDataList = _generatorItemDataSo.spawnableItemDataList;
            var percentage = percentageDataList.Sum(x => x.percentage);

            var randomValue = Random.Range(0f, percentage);
            var cumulative = 0f;

            foreach (var percentageData in percentageDataList)
            {
                cumulative += percentageData.percentage;
                if (randomValue <= cumulative) return percentageData.itemDataSo;
            }

            return null;
        }

        private async UniTask StartRegenerate(float leftDuration)
        {
            regenerateCanvas.SetActive(true);

            DisposeRegenerateToken();
            _regenerateCancellationTokenSource = new CancellationTokenSource();

            await AbilityExtension.Regenerating(leftDuration, _generatorItemDataSo.chargeDuration, imgRegenerate,
                _regenerateCancellationTokenSource.Token);

            regenerateCanvas.SetActive(false);
            RefillSpawnAmount();
        }

        private void StopRegenerate()
        {
            DisposeRegenerateToken();
            regenerateCanvas.SetActive(false);
        }

        private void DisposeRegenerateToken()
        {
            if (_regenerateCancellationTokenSource is not { IsCancellationRequested: false }) return;

            _regenerateCancellationTokenSource.Cancel();
            _regenerateCancellationTokenSource.Dispose();
            _regenerateCancellationTokenSource = null;
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