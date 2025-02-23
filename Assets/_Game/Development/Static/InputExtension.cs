using System;
using UnityEngine;

namespace _Game.Development.Static
{
    public static class InputExtension
    {
        public static Action<Vector2> MouseUpRequest { get; set; }
        public static Action<Vector2> MouseDownRequest { get; set; }
        public static Action<Vector2> MouseDragRequest { get; set; }
    }
}