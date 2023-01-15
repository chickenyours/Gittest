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
            RegisterModel<IUIModel>(new UIModel());
            RegisterSystem<ICameraSystem>(new CameraSystem());
            RegisterSystem<IAudioMgrSystem>(new AudioSystem());
            RegisterSystem<IObjectPoolSystem>(new ObjectpoolSystem());
            RegisterSystem<ITimeSystem>(new TimeSystem());
            RegisterSystem<IInputDeviceMgrSystem>(new InputDeviceMgrSystem());
            RegisterSystem<IPlayerInputSystem>(new PlayerInputSystem());
        }
    }
    //继承IControll的便利形式
    public class GameControll : MonoBehaviour, IController
    {
        IArchitecture IBelongToArchitecture.GetArchitecture() => Game.Interface;
    }
}
