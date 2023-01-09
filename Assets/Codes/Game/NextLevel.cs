using System;
using QFramework;
using UnityEngine;

public class NextLevel : MonoBehaviour,IController
{
    private void Start()
    {
        gameObject.SetActive(false);
        this.RegisterEvent<ShowPassEvent>(OnCanGamePass)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    private void OnCanGamePass(ShowPassEvent e)
    {
        gameObject.SetActive(true);
    }

    public IArchitecture GetArchitecture()
    {
        return Game.Interface;
    }
}
