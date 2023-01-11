using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework
{
    public class ComponentPool<T> where T : Behaviour
    {
        /// <summary>
        /// 用于管理组件的根对象
        /// </summary>
        private GameObject mroot;
        /// <summary>
        /// 指定组件的名字
        /// </summary>
        private string mrootName;
        /// <summary>
        /// 存放所有未激活的组件
        /// </summary>
        private Queue<T> CloseList = new Queue<T>();
        /// <summary>
        /// 存放所有激活的组件
        /// </summary>
        private List<T> OpenList = new List<T>();
        /// <summary>
        /// 获取组件的名字
        /// </summary>
        /// <param name="name"></param>
        public ComponentPool(string name)
        {
            mrootName = name;
        }
        /// <summary>
        /// 设置所有已激活组件
        /// </summary>
        /// <param name="callBack"></param>
        public void SetAllComponent(Action<T> callBack)
        {
            foreach (T component in OpenList) callBack(component);
        }
        /// <summary>
        /// 获取一个组件并激活
        /// </summary>
        /// <param name="component"></param>
        public void Get(out T component)
        {
            if (CloseList.Count > 0)
            {
                component = CloseList.Dequeue(); // 从组件队列中获取一个未组件
                component.enabled = true;        // 激活组件  
            }
            else
            {
                if (mroot == null)
                {
                    mroot = new GameObject(mrootName);
                    GameObject.DontDestroyOnLoad(mroot);
                }
                component = mroot.AddComponent<T>();
            }
            OpenList.Add(component);
        }
        /// <summary>
        /// 自动根据bool条件回收组件
        /// </summary>
        /// <param name="condition"></param>
        public void AutoPush(Func<T, bool> condition)
        {
            for (int i = OpenList.Count - 1; i >= 0; i--)
            {
                if (condition(OpenList[i]))
                {
                    OpenList[i].enabled = false;
                    CloseList.Enqueue(OpenList[i]);
                    OpenList.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// 回收单个组件（注意是在已激活组件中）
        /// </summary>
        /// <param name="component"></param>
        /// <param name="callBack"></param>
        public void Push(T component,Action callBack = null)
        {
            if (OpenList.Contains(component))
            {
                callBack?.Invoke();             //在该组件失活之前做收尾工作（一个回调函数）
                OpenList.Remove(component);
                component.enabled = false;
                CloseList.Enqueue(component);
            }
        }
    }
}