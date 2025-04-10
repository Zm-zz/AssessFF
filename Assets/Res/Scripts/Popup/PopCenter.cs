using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PopKey
{
    // 弹出
    PopUpRobotTip,
    PopUpRobotTipByButton,
    PopUpLongProgressBar,
    PopUpTipWindow,

    // 弹入
    PopDownRobotTip,
}

/// <summary>
/// 弹出弹入注册中心
/// </summary>
public class PopCenter : MonoBehaviour
{
    private void Start()
    {
        // 弹出
        EventCenterManager.AddListener<string>(PopKey.PopUpRobotTip, PopUpRobotTip);
        EventCenterManager.AddListener<string, UnityAction, UnityAction, string, string>(PopKey.PopUpRobotTipByButton, PopUpRobotTipByButton);
        EventCenterManager.AddListener<float, UnityAction, string, UnityAction, bool>(PopKey.PopUpLongProgressBar, PopUpLongProgressBar);
        EventCenterManager.AddListener<UnityAction, UnityAction, UnityAction, string, string, string>(PopKey.PopUpTipWindow, PopUpTipWindow);

        // 弹入
        EventCenterManager.AddListener(PopKey.PopDownRobotTip, PopDownRobotTip);
    }

    #region Robot Show Tips

    private void PopUpRobotTip(string content)
    {
        Robot.Instance.ShowTips(content);
    }

    private void PopUpRobotTipByButton(string content, UnityAction actL = null, UnityAction actR = null, string lContent = "是", string rContent = "否")
    {
        Robot.Instance.ShowTipByBut(content, actL, actR, lContent, rContent);
    }

    private void PopDownRobotTip()
    {
        Robot.Instance.HideTips();
    }

    #endregion

    #region Long Progress Bar

    [Space()]
    [Header("长进度条")]
    public GameObject pre_LongProgressBar;
    public Transform trans_LongProgressParent;

    private void PopUpLongProgressBar(float loadTime, UnityAction completeAction = null, string customContent = "查看中", UnityAction buttonAction = null, bool autoRecovery = false)
    {
        LongProgressBar longProgressBar = ObjectPoolsManager.Instance.Spawn(pre_LongProgressBar, trans_LongProgressParent).GetComponent<LongProgressBar>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(longProgressBar.GetComponent<RectTransform>());
        longProgressBar.GetComponent<RectTransform>().anchoredPosition = pre_LongProgressBar.GetComponent<RectTransform>().anchoredPosition;
        longProgressBar.ReRun(loadTime, completeAction, customContent, buttonAction, autoRecovery);
    }

    #endregion

    #region Tip Pop Window

    [Space()]
    [Header("提示弹窗")]
    public GameObject pre_TipPopWindow;
    public Transform trans_TipPopWindowParent;

    private void PopUpTipWindow(UnityAction actionYes = null, UnityAction actionNo = null, UnityAction actionBoth = null, string info = "是否确认退出", string textYes = "确定", string textNo = "取消")
    {
        PopPanelConfirm popupWindow = ObjectPoolsManager.Instance.Spawn(pre_TipPopWindow, trans_TipPopWindowParent).GetComponent<PopPanelConfirm>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupWindow.GetComponent<RectTransform>());
        popupWindow.GetComponent<RectTransform>().anchoredPosition = pre_TipPopWindow.GetComponent<RectTransform>().anchoredPosition;
        popupWindow.ReRun(actionYes, actionNo, actionBoth, info, textYes, textNo);
    }

    #endregion
}
