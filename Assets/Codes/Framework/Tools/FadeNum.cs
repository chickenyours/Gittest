using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QFramework
{
    /// <summary>
    /// 定义Fade状态值
    /// </summary>
    public enum FadeState
    {
        Close,
        /// <summary>
        /// 淡入(0 -> 1)
        /// </summary>
        FadeIn,
        /// <summary>
        /// 淡出(1 -> 0)
        /// </summary>
        FadeOut
    }
    public class FadeNum
    {
        /// <summary>
        /// 默认原状态为CLOSE
        /// </summary>
        private FadeState mFadeState = FadeState.Close;
        /// <summary>
        /// 公开一个只读的表示是否运行状态值
        /// </summary>
        public bool IsEnable => mFadeState != FadeState.Close;
        /// <summary>
        /// 初始化判断值
        /// </summary>
        private bool mInit = false;
        /// <summary>
        /// 储存当前淡化值，并公开一个只读值
        /// </summary>
        private float mCorrentTime;
        public float CorrentValue => mCorrentTime;
        /// <summary>
        /// 定义委托，在当前运行转为关闭时回调
        /// </summary>
        private Action mOnevent;
        /// <summary>
        /// 定义临界值
        /// </summary>
        private float max = 1f, min = 0f;
        /// <summary>
        /// 公开一个可修改临界值的方法
        /// </summary>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        public void SetMaxMin(float maxValue,float minValue)
        {
            max = maxValue;
            min = minValue;
        }
        /// <summary>
        /// 公开一个可修改状态值的方法，并初始化
        /// </summary>
        /// <param name="fadeState"></param>
        /// <param name="callBack"></param>
        public void SetState(FadeState fadeState,Action callBack = null)
        {
            mFadeState = fadeState;
            mOnevent = callBack;
            mInit = false;
        }
        /// <summary>
        /// 每帧执行渐入淡化操作，step参数决定改变幅度
        /// </summary>
        /// <param name="step"></param>
        public void Update(float step)
        {
            switch (mFadeState)
            {
                //渐入状态 0 -> 1
                case FadeState.FadeIn:
                    if(!mInit)
                    {
                        mCorrentTime = min;
                        mInit = true;
                    }
                    if(mCorrentTime < max)
                    {
                        mCorrentTime += step;
                    }
                    else
                    {
                        mCorrentTime = max;
                        if (mInit) mFadeState = FadeState.Close;
                        mOnevent?.Invoke();
                    }
                    break;
                //渐出状态 1 -> 0
                case FadeState.FadeOut:
                    if (!mInit)
                    {
                        mCorrentTime = max;
                        mInit = true;
                    }
                    if (mCorrentTime > min)
                    {
                        mCorrentTime -= step;
                    }
                    else
                    {
                        mCorrentTime = min;
                        if (mInit) mFadeState = FadeState.Close;
                        mOnevent?.Invoke();
                    }
                    break;
            }
        }
    }
}
