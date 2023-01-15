using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace QFramework
{
    public interface IObjectPoolSystem : ISystem
    {
        GameObject Get(string name);
        void Get(string name, Action<GameObject> callBack = null);
        void Recovery(GameObject obj);
        /// <summary>
        /// 清空对象池，多用于场景切换中
        /// </summary>
        void DisPose();
    }
    public class ObjectpoolSystem : AbstractSystem, IObjectPoolSystem
    {
        /// <summary>
        /// 用于储存缓存池的字典型对象池
        /// </summary>
        private Dictionary<string, PoolData> mPoolDic;
        /// <summary>
        /// 用于挂在对象池的根节点
        /// </summary>
        private Transform mPoolRoot;
        protected override void OnInit()
        {
            mPoolDic = new Dictionary<string, PoolData>();
        }
        /// <summary>
        /// 同步加载获取一个GameObject
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        GameObject IObjectPoolSystem.Get(string name)
        {
            return mPoolDic.TryGetValue(name, out PoolData data) && data.canGet ? data.Get() : ResHelp.SyncLoad<GameObject>(name); 
        }
        /// <summary>
        /// 异步加载获取一个GameObject(Resource根目录下查找)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callBack"></param>
        void IObjectPoolSystem.Get(string name, Action<GameObject> callBack )
        {
            if(mPoolDic.TryGetValue(name, out PoolData data) && data.canGet)
            {
                if (callBack == null) data.Get();
                else callBack(data.Get());
                return;
            }
            ResHelp.LoadAsync<GameObject>(name, o =>
             {
                 if (!mPoolRoot) mPoolRoot = new GameObject("PoolRoot").transform;
                 o.name = name;
                 callBack?.Invoke(o);
             });
        }
        /// <summary>
        /// 将GameObject加入对象池，若无缓存池则创建缓存池
        /// </summary>
        /// <param name="obj"></param>
        void IObjectPoolSystem.Recovery(GameObject obj)
        {
            if (mPoolDic.TryGetValue(obj.name, out var data))
            {
                data.Push(obj);
                return;
            }
            if (!mPoolRoot) mPoolRoot = new GameObject("PoolRoot").transform;
            mPoolDic.Add(obj.name, new PoolData(obj,mPoolRoot));
        }
        /// <summary>
        /// 清空对象池，多用于场景切换中
        /// </summary>
        void IObjectPoolSystem.DisPose()
        {
            mPoolDic.Clear();
            mPoolRoot = null;
        }
    }
    /// <summary>
    /// 缓存池
    /// </summary>
    public class PoolData
    {
        /// <summary>
        /// 可激活对象队列
        /// </summary>
        private Queue<GameObject> mActivatbleObject = new Queue<GameObject>();
        /// <summary>
        /// 是否可以获取对象标识
        /// </summary>
        public bool canGet => mActivatbleObject.Count > 0;
        /// <summary>
        /// 对象挂载父节点
        /// </summary>
        private Transform mFatherObj;
        /// <summary>
        /// 缓存池的初始化
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="root"></param>
        public PoolData(GameObject obj,Transform root)
        {
            mFatherObj = new GameObject(obj.name).transform;
            mFatherObj.SetParent(root.transform);
            Push(obj);
        }
        /// <summary>
        /// 在缓存池中获取一个GameObject
        /// </summary>
        /// <returns></returns>
        public GameObject Get()
        {
            GameObject obj = mActivatbleObject.Dequeue();
            obj.SetActive(true);
            obj.transform.SetParent(null);
            return obj;
        }
        /// <summary>
        /// 将外部GameObject收入对象池中
        /// </summary>
        /// <param name="obj"></param>
        public void Push(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.SetParent(mFatherObj);
            mActivatbleObject.Enqueue(obj);
        }
    }
}
