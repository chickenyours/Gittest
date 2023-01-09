using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework 
{
    public class Timer
    {
        //计时完成时执行委托
        private Action<Timer> mOnFinished;
        //计时的长度
        private float mDelayTime;
        private float mFinishedTime;
        //是否循环计时
        private bool mIsLoop;
        //是否停止计时，并公布是否完成计时的标识
        private bool mIsFinished;
        public bool IsFinished => mIsFinished;
        /// <summary>
        /// 开始计时(计时器初始化操作)
        /// </summary>
        /// <param name="onFinished"></param>
        /// <param name="delayTime"></param>
        /// <param name="isLoop"></param>
        public void Start(float delayTime,bool isLoop, Action<Timer> onFinished)
        {
            mIsFinished = false;
            mOnFinished = null;
            mOnFinished = onFinished;
            mFinishedTime = Time.time + delayTime;
            mDelayTime = delayTime;
            mIsLoop = isLoop;
        }
        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop() => mIsFinished = true;
        /// <summary>
        /// 更新时间器
        /// </summary>
        public void Update()
        {
            if (mIsFinished) return;
            if (mFinishedTime >= Time.time) return;
            if (!mIsLoop) Stop();
            else mFinishedTime = Time.time + mDelayTime;
            mOnFinished?.Invoke(this);
            mOnFinished = null;
        }
    }
    public class TimeMgr
    {
        private Queue<Timer> mTimerPool = new Queue<Timer>();
        private List<Timer> mAvailableTimerList = new List<Timer>();
        /// <summary>
        /// 向计时器对象池添加计时器(计时器生成计时)返回计时器
        /// </summary>
        /// <param name="onFinishedTime">完成计时执行委托</param>
        /// <param name="delayTime"></param>
        /// <param name="isLoop"></param>
        /// <returns></returns>
        public Timer Add(float delayTime,bool isLoop, Action<Timer> onFinishedTime)
        {
            Timer o = mTimerPool.Count == 0 ? new Timer() : mTimerPool.Dequeue();
            mAvailableTimerList.Add(o);
            o.Start(delayTime, isLoop, onFinishedTime);
            return o;
        }
        public void Recover(Timer timer)
        {
            if(mAvailableTimerList.Contains(timer))
            {
                mAvailableTimerList.Remove(timer);
                mTimerPool.Enqueue(timer);
            }
        }
        /// <summary>
        /// 更新所有可用计时器，完成计时的计时器将移除对象池
        /// </summary>
        public void Update()
        {
            if (mAvailableTimerList.Count == 0) return;
            for (int i = mAvailableTimerList.Count - 1; i >= 0; i--)
            {
                Timer o = mAvailableTimerList[i];
                if (o.IsFinished)
                {
                    mAvailableTimerList.RemoveAt(i);
                    mTimerPool.Enqueue(o);
                    continue;
                }
                o.Update();
            }
        }
        public void DisPose()
        {
            mTimerPool.Clear();
            mAvailableTimerList.Clear();
        }
    }
}

