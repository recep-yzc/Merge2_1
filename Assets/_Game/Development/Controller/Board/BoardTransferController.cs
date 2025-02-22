using System;
using System.Collections.Generic;
using _Game.Development.Enum.Board;
using _Game.Development.Interface.Ability;
using _Game.Development.Interface.Item;
using _Game.Development.Scriptable.Ability;
using _Game.Development.Serializable.Grid;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Game.Development.Controller.Board
{
    public class BoardTransferController : MonoBehaviour
    {
        #region Unity Action

        private void Start()
        {
            _transferActionDict.Add(TransferAction.Move, Move);
            _transferActionDict.Add(TransferAction.Swap, Swap);
        }

        #endregion

        private void Move(params object[] parameters)
        {
            if (parameters[0] is not GridData gridData) return;

            gridData.GetComponent<IMoveable>().MoveAsync(gridData.coordinate, _moveDataSo).Forget();

        }

        private void Swap(params object[] parameters)
        {
            if (parameters[0] is not GridData gridDataFirst || parameters[1] is not GridData gridDataSecond) return;

            (gridDataSecond.item, gridDataFirst.item) = (gridDataFirst.item, gridDataSecond.item);
            (gridDataSecond.itemDataSo, gridDataFirst.itemDataSo) = (gridDataFirst.itemDataSo, gridDataSecond.itemDataSo);
            
            if (gridDataFirst.item is not null)
            {
                gridDataFirst.GetComponent<IMoveable>().MoveAsync(gridDataFirst.coordinate, _moveDataSo).Forget();
            }

            if (gridDataSecond.item is not null)
            {
                gridDataSecond.GetComponent<IMoveable>().MoveAsync(gridDataSecond.coordinate, _moveDataSo).Forget();
            }
        }

        public void TryTransfer(TransferAction transferAction, params object[] parameters)
        {
            _transferActionDict[transferAction]?.Invoke(parameters);
        }

        #region Parameters

        [Inject] private MoveDataSo _moveDataSo;
        private readonly Dictionary<TransferAction, Action<object[]>> _transferActionDict = new();

        #endregion
    }
}