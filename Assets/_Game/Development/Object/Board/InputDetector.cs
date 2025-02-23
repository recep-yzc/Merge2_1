using _Game.Development.Enum.Board;
using _Game.Development.Static;
using UnityEngine;

namespace _Game.Development.Object.Board
{
    public class InputDetector : MonoBehaviour
    {
        #region Unity Action

        private void Update()
        {
            if (!BoardExtension.Parameters.IsBoardInitialized) return;
            if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonUp(0))
                return;

            var mousePosition = Input.mousePosition;
            switch (GetMouseAction())
            {
                case MouseAction.Down:
                    InputExtension.MouseDownRequest?.Invoke(mousePosition);
                    break;
                case MouseAction.Hold:
                    InputExtension.MouseDragRequest?.Invoke(mousePosition);
                    break;
                case MouseAction.Up:
                    InputExtension.MouseUpRequest?.Invoke(mousePosition);
                    break;
            }
        }

        #endregion

        #region Getter Setter

        private MouseAction GetMouseAction()
        {
            if (Input.GetMouseButtonDown(0)) return MouseAction.Down;
            if (Input.GetMouseButton(0)) return MouseAction.Hold;
            if (Input.GetMouseButtonUp(0)) return MouseAction.Up;
            return MouseAction.None;
        }

        #endregion
    }
}