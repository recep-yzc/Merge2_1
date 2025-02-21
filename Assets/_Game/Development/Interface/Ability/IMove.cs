using _Game.Development.Scriptable.Ability;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Game.Development.Interface.Ability
{
    public interface IMove
    {
        public UniTask MoveAsync(Vector2 coordinate ,MoveDataSo moveDataSo);
    }
}