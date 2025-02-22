using UnityEngine;

namespace _Game.Development.Static
{
    public static class CameraExtension
    {
        public static Vector2 GetCameraPosition(this Camera camera)
        {
            Vector2 mousePosition = Input.mousePosition;
            return camera.ScreenToWorldPoint(mousePosition);
        }
    }
}