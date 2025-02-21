using _Game.Development.Abstract.Board;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Game.Development.Object.Board
{
    public class SelectedGrid : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private SpriteRenderer sprIcon;

        [Header("Properties")] [SerializeField]
        private float duration = 0.15f;

        [SerializeField] private float force = 0.9f;
        [SerializeField] private Ease ease = Ease.InOutQuad;

        private void RequestChangeVisibility(bool isVisible)
        {
            sprIcon.enabled = isVisible;
        }

        private void RequestSetPosition(Vector2 position)
        {
            transform.position = position;
        }

        private async UniTask RequestScaleUpDown()
        {
            await transform.DOScale(force, duration)
                .From(1)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject)
                .SetEase(ease)
                .AsyncWaitForCompletion();
        }

        #region Unity Action

        private void OnEnable()
        {
            BoardActions.Selector.RequestChangeVisibility += RequestChangeVisibility;
            BoardActions.Selector.RequestSetPosition += RequestSetPosition;
            BoardActions.Selector.RequestScaleUpDown += RequestScaleUpDown;
        }

        private void OnDisable()
        {
            BoardActions.Selector.RequestChangeVisibility -= RequestChangeVisibility;
            BoardActions.Selector.RequestSetPosition -= RequestSetPosition;
            BoardActions.Selector.RequestScaleUpDown -= RequestScaleUpDown;
        }

        #endregion
    }
}