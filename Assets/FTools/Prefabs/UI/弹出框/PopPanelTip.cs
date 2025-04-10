using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopPanelTip : MonoBehaviour
{
    public GameObject panel;
    public Text title;
    public Text info;
    public Text textBtn;
    public Button btn;

    public void PopPanelWithTitle(string title,string info,UnityAction actionOK = null,string btnText = "确定")
    {
        this.title.text = title;
        PopPanel(info,actionOK,btnText);
    }
    public void PopPanel(string info,UnityAction actionOK = null,string btnText = "确定")
    {
        this.info.text = info;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => { actionOK?.Invoke(); });
        btn.onClick.AddListener(() => { HidePanel(); });
        textBtn.text = btnText;
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }
}
