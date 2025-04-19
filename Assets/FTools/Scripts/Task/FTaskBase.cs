using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FTaskBase : MonoBehaviour
{
    public int taskIndex;
    public string taskName;

    protected Coroutine actionCor;
    protected bool onAction;
    protected int actionCorLock;

    public virtual void InitTask()
    {

    }

    public virtual void ResetTask()
    {

    }

    public virtual void EnterTask()
    {
        FDatas.curIndex_������� = taskIndex;
        EventCenterManager.Broadcast(TaskCommand.Enter_Before, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Enter, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Enter_After, taskIndex);
        onAction = true;
        Debug.Log($"<size=13><color=green>��������({taskIndex})��</color></size>{taskName}");

        Robot.Instance.isTip = false;
    }

    public virtual void ExitTask()
    {
        StopActionCor();
        onAction = false;
        EventCenterManager.Broadcast(TaskCommand.Exit_Before, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Exit, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Exit_After, taskIndex);
        // Debug.Log($"<color=cyan>�뿪����({taskIndex})��{taskName}</color>");

        ModelCtrl.Instance.SetInteractiveModelsHighlight(false);
    }

    public virtual void StopTask()
    {
        if (FDatas.curIndex_������� == taskIndex)
        {
            ExitTask();
        }
    }

    public void StartActionCor(string cor)
    {
        if (actionCor != null)
        {
            StopCoroutine(actionCor);
        }
        actionCor = StartCoroutine(cor);
    }

    public void StartActionCor(IEnumerator cor)
    {
        if (actionCor != null)
        {
            StopCoroutine(actionCor);
        }
        actionCor = StartCoroutine(cor);
    }

    public void StopActionCor()
    {
        if (actionCor != null)
        {
            StopCoroutine(actionCor);
        }
    }

    protected void NextTask()
    {
        GameManager.Instance.EnterTask(taskIndex + 1);
    }

    /// <summary>
    /// ��ȡȥ�������ŵ����ֳ���
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public int GetLengthWithoutPunctuation(string text)
    {
        // ��������ŵ��ַ���
        string punctuations = ".����������\"\'";

        // ʹ�� LINQ ȥ�����еı�����
        string textWithoutPunctuation = new string(text.Where(c => !punctuations.Contains(c)).ToArray());

        // ����ȥ�������ź���ַ�������
        return textWithoutPunctuation.Length;
    }

    /// <summary>
    /// �رյ�ǰ��������Ľ���
    /// </summary>
    protected void CloseCurrentModelInteraction()
    {
        string key = ModelCtrl.Instance.currClickModel.gameObject.name;
        Debug.Log($"�رս�����<color=green>{key}</color>");

        ModelCtrl.Instance.Get(key).enable = false;
        ModelCtrl.Instance.Get(key).highlight.SetHighlighted(false);
    }

    protected void OpenCurrentModelInteraction()
    {
        string key = ModelCtrl.Instance.currClickModel.gameObject.name;
        Debug.Log($"����������<color=green>{key}</color>");

        ModelCtrl.Instance.Get(key).enable = true;
        ModelCtrl.Instance.Get(key).highlight.SetHighlighted(false);
    }
}
