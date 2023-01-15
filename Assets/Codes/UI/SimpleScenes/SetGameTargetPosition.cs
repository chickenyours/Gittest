using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class SetGameTargetPosition : MonoBehaviour, IController
{
    void Start()
    {
        transform.position = new Vector3(
            this.GetModel<IGameModel>().mousePosition.Value.x,
            this.GetModel<IGameModel>().mousePosition.Value.y,
            0f);
    }
    void LateUpdate()
    {
        transform.position = new Vector3(
            this.GetModel<IGameModel>().mousePosition.Value.x,
            this.GetModel<IGameModel>().mousePosition.Value.y,
            0f);
    }


    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return Game.Interface;
    }
}
