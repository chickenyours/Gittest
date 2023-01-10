using System;
using QFramework;
using UnityEngine;

public class NextLevel : GameControll
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

}
