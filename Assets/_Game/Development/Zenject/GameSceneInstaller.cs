using _Game.Development.Controller.Board;
using _Game.Development.Factory.Item;
using Zenject;

namespace _Game.Development.Zenject
{
    public class GameSceneInstaller : MonoInstaller<GameSceneInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInstance(FindObjectOfType<GeneratorFactory>());
            Container.BindInstance(FindObjectOfType<ProductFactory>());


            Container.BindInstance(FindObjectOfType<BoardController>());
            Container.BindInstance(FindObjectOfType<BoardLoadController>());


            Container.BindInstance(FindObjectOfType<BoardTransferController>());
            Container.BindInstance(FindObjectOfType<BoardMergeController>());
        }
    }
}