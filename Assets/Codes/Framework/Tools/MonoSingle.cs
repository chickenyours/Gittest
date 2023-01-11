using System;
using UnityEngine;

namespace QFramework
{
    //Monopublic单例,用于全局.当避免场景切换时单例的生成和销毁，保证单例始终运行
    public abstract class MonoSingle<T> : MonoBehaviour where T : MonoBehaviour
    {
        //确保该单例始终为一个，且以GameObject形式存在
        private static T mInstance;
        public static T Instance
        {
            get
            {
                if(mInstance == null)
                {
                    var o = new GameObject(typeof(T).Name);
                    mInstance = o.AddComponent<T>();
                    GameObject.DontDestroyOnLoad(o);
                }
                return mInstance;
            }
        }
    }
}
