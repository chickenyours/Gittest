using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace QFramework
{
    public class PublicMono : MonoSingle<PublicMono>,IController
    {
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