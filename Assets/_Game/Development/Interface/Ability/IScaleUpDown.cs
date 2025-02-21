using _Game.Development.Scriptable.Ability;
using Cysharp.Threading.Tasks;

namespace _Game.Development.Interface.Ability
{
    public interface IScaleUpDown
    {
        public UniTaskVoid ScaleUpDownAsync(ScaleUpDownDataSo scaleUpDownDataSo);
    }
}