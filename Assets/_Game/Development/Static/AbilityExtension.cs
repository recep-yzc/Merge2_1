using System.Threading;
using _Game.Development.Scriptable.Ability;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Game.Development.Static
{
    public static class AbilityExtension
    {
        public static async UniTaskVoid ScaleUpDownHandle(Transform transform, ScaleUpDownDataSo scaleUpDownDataSo,
            CancellationToken cancellationToken)
        {
            var duration = scaleUpDownDataSo.duration;
            var animationCurve = scaleUpDownDataSo.animationCurve;
            var force = scaleUpDownDataSo.force;

            transform.localScale = Vector3.one;

            var elapsedTime = 0f;
            var startScale = transform.localScale;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var normalizedTime = elapsedTime / duration;
                var curveValue = animationCurve.Evaluate(normalizedTime);
                var scale = startScale + curveValue * force;

                transform.localScale = scale;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }

        public static async UniTask MoveHandle(Transform transform, Vector2 targetPosition, MoveDataSo moveDataSo,
            CancellationToken cancellationToken)
        {
            var duration = moveDataSo.duration;
            var animationCurve = moveDataSo.animationCurve;

            Vector2 startPosition = transform.position;
            var elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var normalizedTime = elapsedTime / duration;
                var curveValue = animationCurve.Evaluate(normalizedTime);
                var newPosition = startPosition + (targetPosition - startPosition) * curveValue;

                transform.position = newPosition;
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            transform.position = targetPosition;
        }


        public static async UniTask Regenerating(float duration, float totalDuration, Image imgBar,
            CancellationToken cancellationToken)
        {
            var elapsedTime = totalDuration - duration;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                var normalizedTime = elapsedTime / duration;
                imgBar.fillAmount = normalizedTime;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}