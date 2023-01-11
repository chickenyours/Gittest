using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using QFramework;

namespace QFramework
{
    public class ResPool<T> where T: UnityEngine.Object
    {
        private Dictionary<string,T> pool = new Dictionary<string,T>();
        /// <summary>
        /// 获取ReSource目录资源，若资源池中未加载，则通过异步处理使其加载至资源池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="callBack"></param>
        public void Get(string key,Action<T> callBack)
        { 
            if(pool.TryGetValue(key,out T data))
            {
                callBack(data);
                return;
            }
            ResHelp.LoadAsync<T>(key, o =>
             {
                 callBack(o);
                 pool.Add(key, o);
             });
        }
        /// <summary>
        /// 清理资源池
        /// </summary>
        public void Clear() { pool.Clear(); }
    }
}
