using UnityEngine;

namespace _Game.Development.Static
{
    public static class CameraExtension
    {
        public static Vector2 GetCameraPosition(this Camera camera)
        {
            var mousePosition = Input.mousePosition;
            mousePosition.z = camera.transform.position.z;
            return camera.ScreenToWorldPoint(mousePosition);
        }
    }
}