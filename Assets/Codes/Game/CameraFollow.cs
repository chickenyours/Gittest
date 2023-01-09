using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraMove
{
    public class CameraFollow : MonoBehaviour
    {
        private Transform followTarget;
        private void Start()
        {
            followTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
        private void LateUpdate()
        {
            this.transform.localPosition = new Vector3( followTarget.localPosition.x,followTarget.localPosition.y,-10);
        }
    }
}


