using QFramework;
using UnityEngine;

namespace QFramework
{
    public class Game : Architecture<Game>
    {
        protected override void Init()
        {
            RegisterModel<IAudioModel>(new AudioModel());
            RegisterModel<IGameModel>(new GameModel());
            RegisterSystem<ICameraSystem>(new CameraSystem());
            RegisterSystem<IAudioMgrSystem>(new AudioSystem());
            RegisterSystem<IObjectPoolSystem>(new ObjectpoolSystem());
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterModel<IUIModel>(new UIModel());
        }
    }
    //继承IControll的便利形式
    public class GameControll : MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture() => Game.Interface;
    }
}
