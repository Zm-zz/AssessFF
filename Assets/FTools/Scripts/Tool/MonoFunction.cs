using UnityEngine.Events;

public class MonoFunction : SingletonPatternMonoBase<MonoFunction>
{
    public PopPanelConfirm panel_ȷ�ϵ���;
    public PopPanelTip panel_��ʾȷ�ϵ���;
    public BoxFade panel_��ʾ����;
    public BoxFade boxFade;

    public void ExitApp()
    {
        // panel_ȷ�ϵ���.PopPanel("�Ƿ�ȷ���˳������","ȷ��","ȡ��", () => { FMethod.Exit(); }, () => { });

        EventCenterManager.Broadcast<UnityAction, UnityAction, UnityAction, string, string, string>
            (PopKey.PopUpTipWindow, () => { FMethod.Exit(); }, () => { }, () => { }, "�Ƿ�ȷ���˳������", "ȷ��", "ȡ��");
    }
}
