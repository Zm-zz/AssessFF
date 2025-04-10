using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(3)]
public class UGIONController : MonoBehaviour
{
    public bool showLogo;
    public bool show开屏动画;
    public bool showLicense = true;
    public GameObject[] logos;
    public GameObject obj_开屏动画;
    public GameObject license;
    public Text text_版本号;
    public Text text_左下角;
    public Text text_右下角;

    public GameObject panel_控制面板;
    public GameObject panel_作弊手册;

    private void Awake()
    {
        UGIONPanelOperation operation = UGIONPanelOperationPanel.Instance.LoadOperation();
        if (operation != null)
        {
            showLogo = operation.active_Logo;
            show开屏动画 = operation.active_开屏动画;
            showLicense = operation.active_License;
            text_版本号.text = operation.str_版本号;
            text_左下角.text = operation.str_左下角;
            text_右下角.text = operation.str_右下角;
        }
        else
        {
            Debug.Log("新建UGION控制台");
            operation = new UGIONPanelOperation();
            operation.active_Logo = showLogo;
            operation.active_开屏动画 = show开屏动画;
            operation.active_License = showLicense;
            operation.str_版本号 = text_版本号.text;
            operation.str_左下角 = text_左下角.text;
            operation.str_右下角 = text_右下角.text;
            UGIONPanelOperationPanel.Instance.SaveOperation(operation);
        }

        foreach (var logo in logos)
        {
            logo.SetActive(showLogo);
        }
        obj_开屏动画.SetActive(show开屏动画);
        // license.SetActive(showLicense);
    }

    private void Update()
    {
        if (CheatingInstructions.CompareInstruction("UgionOperation"))
        {
            panel_控制面板.SetActive(true);
            UGIONPanelOperationPanel.Instance.SetOperationPanelData();
        }
        if (CheatingInstructions.CompareInstruction("UgionDictionary"))
        {
            panel_作弊手册.SetActive(true);
        }
    }

    [InspectorButton("手动保存")]
    void SaveInUnity()
    {
        UGIONPanelOperation operation = new UGIONPanelOperation();
        operation.active_Logo = showLogo;
        operation.active_开屏动画 = show开屏动画;
        operation.active_License = showLicense;
        operation.str_版本号 = text_版本号.text;
        operation.str_左下角 = text_左下角.text;
        operation.str_右下角 = text_右下角.text;
        UGIONPanelOperationPanel.Instance.SaveOperation(operation);
    }
}
