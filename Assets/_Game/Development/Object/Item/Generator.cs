using System;
using System.Linq;
using System.Threading;
using _Game.Development.Factory.Item;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Scriptable.Factory;
using _Game.Development.Scriptable.Item;
using _Game.Development.Serializable.Item;
using _Game.Development.Static;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace _Game.Development.Object.Item
{
    public class Generator : Item, IGenerator, IDraggable, IClickable, IScaleUpDown
    {
        [Header("References")] [SerializeField]
        private Canvas regenerateCanvas;

        [SerializeField] private Image imgRegenerate;

        #region Unity Action

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeScaleUpDownTokenSource();
            DisposeRegenerateTokenSource();
        }

        #endregion

        #region Parameters

        private readonly float _dragMoveSpeed = 15f;

        private int _spawnAmount;
        private string _lastUsingDate;
        private GeneratorItemDataSo _generatorItemDataSo;

        private CancellationTokenSource _regenerateCancellationTokenSource;
        private CancellationTokenSource _scaleUpDownCancellationTokenSource;

        #endregion

        #region Item

        public override void LevelUp()
        {
            StopRegenerate();
            ResetLastUsingDate();
            RefillSpawnAmount();
        }

        public override void SetItemDataSo(ItemDataSo itemDataSo)
        {
            _generatorItemDataSo = (GeneratorItemDataSo)itemDataSo;
        }

        public override ItemSaveData CreateEditedItemSaveData()
        {
            var gridData = BoardExtension.GetGridDataByCoordinate(transform.position);

            var coordinate = gridData.Coordinate;
            var itemDataSo = gridData.itemDataSo;
            var itemId = itemDataSo.GetItemId();

            var func = ItemFactory.CreateEditedItemSaveDataByItemId[itemId];
            var editedItemSaveParameters = new EditedSaveParameters(coordinate, itemDataSo,_lastUsingDate);
            return func.Invoke(editedItemSaveParameters);
        }

        public override void FetchItemData()
        {
            CheckGenerateStatus();
        }

        #endregion

        #region Generator

        private void CheckGenerateStatus()
        {
            var lastUseTime = _lastUsingDate.StringToDateTime();
            var elapsedTime = (float)(DateTime.Now - lastUseTime).TotalSeconds;
            var remainingTime = _generatorItemDataSo.regenerateDuration - elapsedTime;

            if (remainingTime <= 0)
            {
                RefillSpawnAmount();
                return;
            }

            StartRegenerate(remainingTime).Forget();
        }

        private void UpdateGenerateStatus()
        {
            var spawnCountOver = _spawnAmount <= 0;
            if (spawnCountOver)
            {
                var regenerateDuration = _generatorItemDataSo.regenerateDuration;
                StartRegenerate(regenerateDuration).Forget();
            }

            _lastUsingDate = DateTime.Now.DateTimeToString();
        }

        private void SetCanvasOrder(sbyte order)
        {
            regenerateCanvas.sortingOrder = order;
        }

        public void SetLastUsingDate(string date)
        {
            _lastUsingDate = date;
        }

        private void ResetLastUsingDate()
        {
            _lastUsingDate = DateTime.Now.AddSeconds(_generatorItemDataSo.regenerateDuration).DateTimeToString();
        }

        public bool CanGenerate()
        {
            return _generatorItemDataSo.spawnableItemDataList.Length > 0;
        }

        public int GetSpawnCount()
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
            UpdateGenerateStatus();
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

        private async UniTask StartRegenerate(float duration)
        {
            SetCanvasVisibility(true);

            DisposeRegenerateTokenSource();
            NewRegenerateTokenSource();
            await AbilityExtension.Regenerating(duration, _generatorItemDataSo.regenerateDuration, imgRegenerate,
                _regenerateCancellationTokenSource.Token);

            SetCanvasVisibility(false);
            RefillSpawnAmount();
        }

        private void SetCanvasVisibility(bool isVisible)
        {
            regenerateCanvas.gameObject.SetActive(isVisible);
        }

        private void StopRegenerate()
        {
            DisposeRegenerateTokenSource();
            SetCanvasVisibility(false);
        }

        private void NewRegenerateTokenSource()
        {
            _regenerateCancellationTokenSource = new CancellationTokenSource();
        }

        private void DisposeRegenerateTokenSource()
        {
            if (_regenerateCancellationTokenSource is not { IsCancellationRequested: false }) return;

            _regenerateCancellationTokenSource.Cancel();
            _regenerateCancellationTokenSource.Dispose();
            _regenerateCancellationTokenSource = null;
        }

        #endregion

        #region Ability

        #region Draggable

        public void Drag(Vector2 vector2)
        {
            transform.position = Vector2.Lerp(transform.position, vector2, Time.deltaTime * _dragMoveSpeed);
        }

        #endregion

        #region Clickable

        public void MouseDown()
        {
            SetSpriteOrder(1);
            SetCanvasOrder(1);
        }

        public void MouseUp()
        {
            SetSpriteOrder(0);
            SetCanvasOrder(0);
        }

        #endregion

        #region ScaleUpDown

        public UniTaskVoid ScaleUpDownAsync(ScaleUpDownDataSo scaleUpDownDataSo)
        {
            DisposeScaleUpDownTokenSource();
            NewScaleUpTokenSource();

            return AbilityExtension.ScaleUpDownHandle(transform, scaleUpDownDataSo,
                _scaleUpDownCancellationTokenSource.Token);
        }

        private void NewScaleUpTokenSource()
        {
            _scaleUpDownCancellationTokenSource = new CancellationTokenSource();
        }

        private void DisposeScaleUpDownTokenSource()
        {
            if (_scaleUpDownCancellationTokenSource is not { IsCancellationRequested: false }) return;

            _scaleUpDownCancellationTokenSource.Cancel();
            _scaleUpDownCancellationTokenSource.Dispose();
            _scaleUpDownCancellationTokenSource = null;
        }

        #endregion

        #endregion
    }
}