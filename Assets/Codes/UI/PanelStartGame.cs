using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

public class PanelStartGame : GameControll
{
    private Button mStartBin;
    private Button mExitBin;
    private void Awake()
    {
        mStartBin = transform.Find("StartBin").GetComponent<Button>();
        mExitBin = transform.Find("ExitBin").GetComponent<Button>();
        mStartBin.onClick.AddListener(OnStartBin);
        mExitBin.onClick.AddListener(OnExitBin);
    }
    private void Start()
    {
        this.GetSystem<IAudioMgrSystem>().PlayBgm("Death By Glamour");
    }
    private void OnStartBin()
    {
        this.SendCommand<LoadScene>(new LoadScene("SimpleScene"));
    }
    private void OnExitBin()
    {
        Application.Quit();
    }
}
