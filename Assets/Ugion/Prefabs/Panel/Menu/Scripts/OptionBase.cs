using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(UIChange))]
public class OptionBase : MonoBehaviour
{
    [Header("---------- IsOn")]
    [ReadOnly][SerializeField] private bool bool_IsOn;
    [ReadOnly] public ProcedureInfo procedureInfo;

    [ReadOnly] public int index = -1;

    private Button but_Self;
    private UIChange _UIChange;
    private FlexSubOptions _FlexSubOptions;

    [HideInInspector]
    public Transform extensionMenu;

    public bool Bool_IsOn
    {
        get => bool_IsOn;
        set
        {
            bool_IsOn = value;
        }
    }

    public virtual void ChangeState(bool isOn)
    {
        Bool_IsOn = isOn;
        _UIChange.ChangeState(isOn);

        if (procedureInfo.hasExtension)
        {
            //extensionMenu.gameObject.SetActive(isOn);
            _FlexSubOptions.Spread(isOn);
        }
        else
        {
            if (isOn)
            {
                // ִ������
                GlobalComponent.Instance.MenuManager.ChangeProcedure(procedureInfo.ProcedureConfig);
            }
        }
    }

    /// <summary>
    /// ��ʼ�� 
    /// clickAction Ϊ null�����ḳֵ
    /// </summary>
    public virtual void Initialize(ProcedureInfo procedureInfo, int index)
    {
        but_Self = GetComponent<Button>();
        _UIChange = GetComponent<UIChange>();
        _FlexSubOptions = transform.parent.GetComponentInChildren<FlexSubOptions>();
        extensionMenu = _FlexSubOptions.transform;

        bool_IsOn = false;

        this.index = index;
        this.procedureInfo = procedureInfo;

        transform.GetComponentInChildren<Text>().text = procedureInfo.ProcedureConfig.procedureTitle;

        but_Self.onClick.RemoveAllListeners();
        but_Self.onClick.AddListener(() => GlobalComponent.Instance.MenuManager.ChangeState(this));
    }
}
