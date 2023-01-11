using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QFramework;
using UnityEngine;

namespace QFramework
{
    public interface ITimeSystem : ISystem
    {
        Timer Add(float delayTime, bool isLoop, Action<Timer> onFinishedTime);
        void Stop(Timer timer);
        void Recover(Timer timer);
        void DisPose();
    }

    public class TimeSystem : AbstractSystem, ITimeSystem
    {
        private TimeMgr mTimeMgr;
        protected override void OnInit()
        {
            mTimeMgr = new TimeMgr();
            PublicMono.Instance.OnFixedUpdate += Update;
            MonoBehaviour.print("build a TimeSystem");
        }
        Timer ITimeSystem.Add(float delayTime, bool isLoop, Action<Timer> onFinishedTime)
        {
            Timer o = mTimeMgr.Add(delayTime, isLoop, onFinishedTime);
            return o;
        }
        public void Update()
        {
            mTimeMgr.Update();
        }
        public void Stop(Timer timer)
        {
            timer.Stop();
        }
        public void Recover(Timer timer)
        {
            mTimeMgr.Recover(timer);
        }
        public void DisPose()
        {
            mTimeMgr.DisPose();
        }
    }
}
