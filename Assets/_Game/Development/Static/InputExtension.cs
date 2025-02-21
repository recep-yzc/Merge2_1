using System;
using UnityEngine;

namespace _Game.Development.Static
{
    public static class InputExtension
    {
        public static Action<Vector2> OnMouseUpEvent { get; set; }
        public static Action<Vector2> OnMouseDownEvent { get; set; }
        public static Action<Vector2> OnMouseDragEvent { get; set; }
    }
}