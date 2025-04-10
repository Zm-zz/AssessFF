using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopPanelConfirm : MonoBehaviour
{
    public GameObject panel;
    public Text title;
    public Text info;
    public Text textYes;
    public Text textNo;
    public Button btnYes;
    public Button btnNo;

    public void ReRun(UnityAction actionYes = null, UnityAction actionNo = null, UnityAction actionBoth = null, string info = "是否确认退出", string textYes = "确定", string textNo = "取消")
    {
        this.info.text = info;
        this.textYes.text = textYes;
        this.textNo.text = string.IsNullOrEmpty(textNo) ? "" : textNo;

        btnNo.gameObject.SetActive(!string.IsNullOrEmpty(textNo));

        btnYes.onClick.RemoveAllListeners();
        btnNo.onClick.RemoveAllListeners();

        btnYes.onClick.AddListener(() => { actionYes?.Invoke(); });
        btnYes.onClick.AddListener(() => { actionBoth?.Invoke(); });
        btnYes.onClick.AddListener(() => { HidePanel(); });
        btnNo.onClick.AddListener(() => { actionNo?.Invoke(); });
        btnNo.onClick.AddListener(() => { actionBoth?.Invoke(); });
        btnNo.onClick.AddListener(() => { HidePanel(); });
    }

    private void HidePanel()
    {
        ObjectPoolsManager.Instance.Despawn(gameObject);
    }
}
