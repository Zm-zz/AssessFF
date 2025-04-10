using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(8)]
public class ToggleManager : MonoBehaviour
{
    public int lastSelectIndex = -1;
    private List<SingleToggle> _toggles;
    public List<SingleToggle> Toggles { get => _toggles; private set => _toggles = value; }

    private ToggleGroup group;

    [Header("�Ƿ�Ĭ��ѡ��")]
    public bool isStartSelect;

    public void Init()
    {
        group = transform.GetComponentInChildren<ToggleGroup>();
        group.allowSwitchOff = true;

        if (Toggles == null)
        {
            Toggles = transform.GetComponentsInChildren<SingleToggle>().ToList();
        }

        for (int i = 0; i < Toggles.Count; i++)
        {
            Toggles[i].Init();
            Toggles[i].UnSelected();
            lastSelectIndex = -1;
            int index = i;
            Toggles[i].Toggle.onValueChanged.RemoveAllListeners();
            Toggles[i].OnSelectAction = null;
            Toggles[i].OnUnSelectAction = null;
            Toggles[i].Toggle.onValueChanged.AddListener((bool value) => OnTogglesValueChanged(value, index));

            AssignEvent();
        }

        if (isStartSelect)
        {
            SelectToggle(0);
        }
    }


    public void SelectToggle(int index)
    {
        Toggles[index].SelectState();
        Toggles[index].Toggle.isOn = true;
    }

    private void AssignEvent()
    {

    }

    public void OnTogglesValueChanged(bool value, int index)
    {
        if (GameManager.Instance.mode && value)
        {
            Debug.Log(Toggles[index].taskIndex);
            Debug.Log(FDatas.curIndex_�������);

            if (index < lastSelectIndex)
            {
                Toggles[lastSelectIndex].Toggle.isOn = true;
                GameManager.Instance.EnterTask(FDatas.curIndex_�������);
                return;
            }

            if (Toggles[index].taskIndex != 0)
            {
                if (Toggles[index].taskIndex < FDatas.curIndex_�������)
                {
                    Toggles[lastSelectIndex].Toggle.isOn = true;
                    return;
                }
            }
        }

        if (value)
        {
            if (lastSelectIndex == index)
            {
                return;
            }

            if (lastSelectIndex != -1)
            {
                Toggles[lastSelectIndex].UnSelected();
            }

            lastSelectIndex = index;
            Toggles[index].Selected();

            // ѡ���˾Ͳ����ٿ�ѡ
            group.allowSwitchOff = false;
        }
        else
        {
            lastSelectIndex = index;
            Toggles[index].UnSelected();
        }
    }

    public void UnInit()
    {
        for (int i = 0; i < Toggles.Count; i++)
        {
            Toggles[i].UnSelected();
            int index = i;
            Toggles[i].Toggle.onValueChanged.RemoveAllListeners();
            Toggles[i].OnSelectAction = null;
            Toggles[i].OnUnSelectAction = null;
        }
    }
}
