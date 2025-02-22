using _Game.Development.Controller.Board;
using _Game.Development.Factory.Item;
using UnityEngine;
using Zenject;

namespace _Game.Development.Zenject
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(FindObjectOfType<Camera>());

            #region Factory

            Container.BindInstance(FindObjectOfType<GeneratorFactory>());
            Container.BindInstance(FindObjectOfType<ProductFactory>());

            #endregion

            #region Board

            Container.BindInstance(FindObjectOfType<BoardEditController>());

            Container.BindInstance(FindObjectOfType<BoardController>());
            Container.BindInstance(FindObjectOfType<BoardLoadController>());
            Container.BindInstance(FindObjectOfType<BoardSaveController>());

            Container.BindInstance(FindObjectOfType<BoardGenerateController>());
            Container.BindInstance(FindObjectOfType<BoardTransferController>());
            Container.BindInstance(FindObjectOfType<BoardMergeController>());
            Container.BindInstance(FindObjectOfType<BoardScaleUpDownController>());

            #endregion
        }
    }
}