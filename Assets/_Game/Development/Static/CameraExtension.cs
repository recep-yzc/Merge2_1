using UnityEngine;

namespace _Game.Development.Static
{
    public static class CameraExtension
    {
        public static Vector2 GetCoordinate(this Camera camera)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = camera.transform.position.y;
            return camera.ScreenToWorldPoint(mousePosition);
        }
    }
}