using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    class ResHelp
    {
        public static T SyncLoad<T>(string name) where T : Object
        {
            T res = Resources.Load<T>(name);
            return res is GameObject ? GameObject.Instantiate(res) : res;
        }
        public static IEnumerator AsynLoadres<T>(string name, System.Action<T> callBack) where T : Object
        {
            var res = Resources.LoadAsync<T>(name);
            while (res.isDone) yield return null;
            callBack(res.asset is GameObject ? GameObject.Instantiate(res.asset) as T:  res.asset as T);
        }
        public static void LoadAsync<T>(string name, System.Action<T> callBack) where T : Object
        {
            PublicMono.instance.StartCoroutine(AsynLoadres(name,callBack));
        }
    }
}