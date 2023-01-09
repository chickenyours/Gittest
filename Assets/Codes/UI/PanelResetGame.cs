using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using QFramework;

public class PanelResetGame : MonoBehaviour,IController
{
    private void Start()
    {
        transform.Find("Button").GetComponent<Button>().onClick.AddListener(ResetGame);
    }
    private void LateUpdate()
    {
    }
    private void ResetGame()
    {
        this.GetModel<IGameModel>().score.Value = 0;
        this.GetModel<IGameModel>().shootTime.Value = 1;
        this.SendCommand<LoadScene>(new LoadScene("SimpleScene"));
    }
    
    IArchitecture IBelongToArchitecture.GetArchitecture()
    {
        return Game.Interface;
    }
}
