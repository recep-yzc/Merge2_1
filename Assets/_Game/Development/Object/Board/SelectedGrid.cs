using _Game.Development.Static;
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
        private float duration;

        [SerializeField] private float force;
        [SerializeField] private Ease ease;

        private void VisibilityRequest(bool isVisible)
        {
            sprIcon.enabled = isVisible;
        }

        private void SetPositionRequest(Vector2 position)
        {
            transform.position = position;
        }

        private async UniTask ScaleUpDownRequest()
        {
            var tween = transform.DOScale(force, duration)
                .From(1)
                .SetLoops(2, LoopType.Yoyo)
                .SetLink(gameObject)
                .SetEase(ease);

            await tween.AsyncWaitForCompletion();
        }

        #region Unity Action

        private void OnEnable()
        {
            BoardExtension.Selector.VisibilityRequest += VisibilityRequest;
            BoardExtension.Selector.SetPositionRequest += SetPositionRequest;
            BoardExtension.Selector.ScaleUpDownRequest += ScaleUpDownRequest;
        }

        private void OnDisable()
        {
            BoardExtension.Selector.VisibilityRequest -= VisibilityRequest;
            BoardExtension.Selector.SetPositionRequest -= SetPositionRequest;
            BoardExtension.Selector.ScaleUpDownRequest -= ScaleUpDownRequest;
        }

        #endregion
    }
}