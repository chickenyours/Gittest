using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;

public class PanelStartGame : GameControll
{
    private void Awake()
    {
        transform.Find("StartBin").GetComponent<Button>().onClick.AddListener(OnStartBin);
        transform.Find("ExitBin").GetComponent<Button>().onClick.AddListener(OnExitBin);
        transform.Find("SettingOpenBin").GetComponent<Button>().onClick.AddListener(OpenSetting);
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
    private void OpenSetting()
    {
        if (SettingPanel.intence == null)
        {
            ResHelp.LoadAsync<GameObject>("Items/UI/SettingPanel", o =>
            {
                o.transform.SetParent(GameObject.Find("Canvas").transform);
                //定义SettingPanel的锚点坐标
                (o.transform as RectTransform).anchoredPosition = Vector2.zero;
            });
        }
    }
}
