using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Abstract.Board
{
    public abstract class BoardActions
    {
        public abstract class Selector
        {
            public static Action<bool> RequestChangeVisibility { get; set; }
            public static Action<Vector2> RequestSetPosition { get; set; }
            public static Func<UniTask> RequestScaleUpDown { get; set; }
        }
    }
}