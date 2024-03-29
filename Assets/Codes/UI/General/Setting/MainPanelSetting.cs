﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

public class MainPanelSetting : GameControll
{
    private void Awake()
    {
        transform.Find("OpenSettingBin").gameObject.GetComponent<Button>().onClick.AddListener(OpenSetting);
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
