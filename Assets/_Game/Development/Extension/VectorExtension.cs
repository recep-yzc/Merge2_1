using System;
using _Game.Development.Grid;
using UnityEngine;

namespace _Game.Development.Extension
{
    public static class VectorExtension
    {
        public static readonly Vector2 HalfSize = new(0.5f, 0.5f);
        public static readonly Vector2 Size = Vector2.one;

        public static bool CheckOverlapWithDot(Vector2 bottomLeft, Vector2 topRight, Vector2 point)
        {
            return point.x > bottomLeft.x && point.x < topRight.x && point.y < topRight.y && point.y > bottomLeft.y;
        }

        public static Vector2 DirectionToVector(this DirectionId directionId)
        {
            return directionId switch
            {
                DirectionId.LeftUp => Vector2.left + Vector2.up,
                DirectionId.Left => Vector2.left,
                DirectionId.LeftDown => Vector2.left + Vector2.down,
                DirectionId.RightUp => Vector2.right + Vector2.up,
                DirectionId.Right => Vector2.right,
                DirectionId.RightDown => Vector2.right + Vector2.down,
                DirectionId.Up => Vector2.up,
                DirectionId.Down => Vector2.down,
                _ => throw new ArgumentOutOfRangeException(nameof(directionId))
            };
        }
    }
}