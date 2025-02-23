using UnityEngine;
using Zenject;

namespace _Game.Development.Scriptable.Ability
{
    [CreateAssetMenu(fileName = "MoveDataSo", menuName = "Game/Board/Item/Ability/Move/Data")]
    public class MoveDataSo : ScriptableObjectInstaller<MoveDataSo>
    {
        public AnimationCurve animationCurve;
        public float duration;

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
}