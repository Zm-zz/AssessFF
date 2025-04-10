using UnityEngine.Events;

public class MonoFunction : SingletonPatternMonoBase<MonoFunction>
{
    public PopPanelConfirm panel_确认弹框;
    public PopPanelTip panel_提示确认弹框;
    public BoxFade panel_提示弹框;
    public BoxFade boxFade;

    public void ExitApp()
    {
        // panel_确认弹框.PopPanel("是否确定退出软件？","确定","取消", () => { FMethod.Exit(); }, () => { });

        EventCenterManager.Broadcast<UnityAction, UnityAction, UnityAction, string, string, string>
            (PopKey.PopUpTipWindow, () => { FMethod.Exit(); }, () => { }, () => { }, "是否确定退出软件？", "确定", "取消");
    }
}
