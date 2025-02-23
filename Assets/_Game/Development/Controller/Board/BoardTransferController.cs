using System;
using System.Collections.Generic;
using _Game.Development.Enum.Board;
using _Game.Development.Interface.Ability;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardTransferController : MonoBehaviour
    {
        private void Move(object[] parameters)
        {
            var gridData = (GridData)parameters[0];
            if (gridData == null) return;

            var moveable = gridData.GetComponent<IMoveable>();
            moveable?.MoveAsync(gridData.Coordinate, _moveDataSo).Forget();
        }

        private void Swap(object[] parameters)
        {
            var gridDataFirst = (GridData)parameters[0];
            var gridDataSecond = (GridData)parameters[1];

            if (gridDataFirst == null || gridDataSecond == null) return;

            (gridDataSecond.item, gridDataFirst.item) = (gridDataFirst.item, gridDataSecond.item);
            (gridDataSecond.itemDataSo, gridDataFirst.itemDataSo) =
                (gridDataFirst.itemDataSo, gridDataSecond.itemDataSo);

            foreach (GridData gridData in parameters)
            {
                var moveable = gridData.GetComponent<IMoveable>();
                moveable?.MoveAsync(gridData.Coordinate, _moveDataSo).Forget();
            }
        }

        public void TryTransfer(TransferAction transferAction, params object[] parameters)
        {
            var action = _transferActionDict[transferAction];
            action?.Invoke(parameters);
        }

        #region Unity Action

        private void Start()
        {
            _transferActionDict.Add(TransferAction.Move, Move);
            _transferActionDict.Add(TransferAction.Swap, Swap);
            _transferActionDict.Add(TransferAction.SpecificMove, SpecificMove);
        }

        private void SpecificMove(object[] parameters)
        {
            var gridData = (GridData)parameters[0];
            var coordinate = (Vector2)parameters[1];
            if (gridData == null) return;

            var moveable = gridData.GetComponent<IMoveable>();
            moveable?.MoveAsync(coordinate, _moveDataSo).Forget();
        }

        #endregion

        #region Parameters

        [Inject] private MoveDataSo _moveDataSo;
        private readonly Dictionary<TransferAction, Action<object[]>> _transferActionDict = new();

        #endregion
    }
}