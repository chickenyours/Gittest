using QFramework;
using UnityEngine.UI;
using UnityEngine;

public class SettingPanel : GameControll
{
    //保存了MianPanelSetting的引用
    private MainPanelSetting MainPanel;
    private void Awake()
    {
        MainPanel = GameObject.Find("MainPanel").GetComponent<MainPanelSetting>();
        transform.Find("BackBin").gameObject.GetComponent<Button>().onClick.AddListener(OnCloseBin);
    }
    private void OnCloseBin()
    {
        MainPanel.isSettingOpen = false;
        Destroy(gameObject);
    }
}
