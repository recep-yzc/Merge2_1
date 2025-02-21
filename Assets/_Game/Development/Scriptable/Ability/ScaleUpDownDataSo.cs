using UnityEngine;
using Zenject;

namespace _Game.Development.Scriptable.Ability
{
    [CreateAssetMenu(fileName = "ScaleUpDownDataSo", menuName = "Game/Core/Board/Item/Ability/ScaleUpDown/Data")]
    public class ScaleUpDownDataSo : ScriptableObjectInstaller<ScaleUpDownDataSo>
    {
        public AnimationCurve animationCurve;
        public float duration;
        public Vector3 force;

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
}