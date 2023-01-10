using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;

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
        }
    }
}
