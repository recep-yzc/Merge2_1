using System.Collections.Generic;
using _Game.Development.Board.Edit.Serializable;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Game.Development.Board.Edit.Scriptable
{
    [CreateAssetMenu(fileName = "BoardDataSo", menuName = "Game/Edit/Board/Data")]
    public class BoardDataSo : ScriptableObject
    {
        [MinValue(2)] [MaxValue(10)] public int rows;
        [MinValue(2)] [MaxValue(10)] public int columns;

        [HideInInspector] public List<GridInspectorData> gridInspectorDataList = new();
    }
}