using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PopKey
{
    // ����
    PopUpRobotTip,
    PopUpRobotTipByButton,
    PopUpLongProgressBar,
    PopUpTipWindow,

    // ����
    PopDownRobotTip,
}

/// <summary>
/// ��������ע������
/// </summary>
public class PopCenter : MonoBehaviour
{
    private void Start()
    {
        // ����
        EventCenterManager.AddListener<string>(PopKey.PopUpRobotTip, PopUpRobotTip);
        EventCenterManager.AddListener<string, UnityAction, UnityAction, string, string>(PopKey.PopUpRobotTipByButton, PopUpRobotTipByButton);
        EventCenterManager.AddListener<float, UnityAction, string, UnityAction, bool>(PopKey.PopUpLongProgressBar, PopUpLongProgressBar);
        EventCenterManager.AddListener<UnityAction, UnityAction, UnityAction, string, string, string>(PopKey.PopUpTipWindow, PopUpTipWindow);

        // ����
        EventCenterManager.AddListener(PopKey.PopDownRobotTip, PopDownRobotTip);
    }

    #region Robot Show Tips

    private void PopUpRobotTip(string content)
    {
        Robot.Instance.ShowTips(content);
    }

    private void PopUpRobotTipByButton(string content, UnityAction actL = null, UnityAction actR = null, string lContent = "��", string rContent = "��")
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
    [Header("��������")]
    public GameObject pre_LongProgressBar;
    public Transform trans_LongProgressParent;

    private void PopUpLongProgressBar(float loadTime, UnityAction completeAction = null, string customContent = "�鿴��", UnityAction buttonAction = null, bool autoRecovery = false)
    {
        LongProgressBar longProgressBar = ObjectPoolsManager.Instance.Spawn(pre_LongProgressBar, trans_LongProgressParent).GetComponent<LongProgressBar>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(longProgressBar.GetComponent<RectTransform>());
        longProgressBar.GetComponent<RectTransform>().anchoredPosition = pre_LongProgressBar.GetComponent<RectTransform>().anchoredPosition;
        longProgressBar.ReRun(loadTime, completeAction, customContent, buttonAction, autoRecovery);
    }

    #endregion

    #region Tip Pop Window

    [Space()]
    [Header("��ʾ����")]
    public GameObject pre_TipPopWindow;
    public Transform trans_TipPopWindowParent;

    private void PopUpTipWindow(UnityAction actionYes = null, UnityAction actionNo = null, UnityAction actionBoth = null, string info = "�Ƿ�ȷ���˳�", string textYes = "ȷ��", string textNo = "ȡ��")
    {
        PopPanelConfirm popupWindow = ObjectPoolsManager.Instance.Spawn(pre_TipPopWindow, trans_TipPopWindowParent).GetComponent<PopPanelConfirm>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(popupWindow.GetComponent<RectTransform>());
        popupWindow.GetComponent<RectTransform>().anchoredPosition = pre_TipPopWindow.GetComponent<RectTransform>().anchoredPosition;
        popupWindow.ReRun(actionYes, actionNo, actionBoth, info, textYes, textNo);
    }

    #endregion
}
