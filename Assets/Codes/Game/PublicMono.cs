using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QFramework
{
    public class PublicMono : MonoBehaviour,IController
    {
        public static PublicMono instance;

        private void Awake()
        {
            if (instance == null || instance == this) instance = this;
            else Destroy(gameObject);
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }


        public event Action OnUpdate;
        public event Action OnFixedUpdate;
        public event Action OnLateUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        IArchitecture IBelongToArchitecture.GetArchitecture()
        {
            return Game.Interface;
        }
    }
}