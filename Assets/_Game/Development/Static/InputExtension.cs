using System;
using UnityEngine;

namespace _Game.Development.Static
{
    public static class InputExtension
    {
        public static Action<Vector2> MouseUpRequested { get; set; }
        public static Action<Vector2> MouseDownRequested { get; set; }
        public static Action<Vector2> MouseDragRequested { get; set; }
    }
}