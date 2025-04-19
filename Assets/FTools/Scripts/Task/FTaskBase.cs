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
        FDatas.curIndex_流程序号 = taskIndex;
        EventCenterManager.Broadcast(TaskCommand.Enter_Before, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Enter, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Enter_After, taskIndex);
        onAction = true;
        Debug.Log($"<size=13><color=green>进入流程({taskIndex})：</color></size>{taskName}");

        Robot.Instance.isTip = false;
    }

    public virtual void ExitTask()
    {
        StopActionCor();
        onAction = false;
        EventCenterManager.Broadcast(TaskCommand.Exit_Before, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Exit, taskIndex);
        EventCenterManager.Broadcast(TaskCommand.Exit_After, taskIndex);
        // Debug.Log($"<color=cyan>离开流程({taskIndex})：{taskName}</color>");

        ModelCtrl.Instance.SetInteractiveModelsHighlight(false);
    }

    public virtual void StopTask()
    {
        if (FDatas.curIndex_流程序号 == taskIndex)
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
    /// 获取去掉标点符号的文字长度
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public int GetLengthWithoutPunctuation(string text)
    {
        // 定义标点符号的字符串
        string punctuations = ".，？！：：\"\'";

        // 使用 LINQ 去除所有的标点符号
        string textWithoutPunctuation = new string(text.Where(c => !punctuations.Contains(c)).ToArray());

        // 返回去除标点符号后的字符串长度
        return textWithoutPunctuation.Length;
    }

    /// <summary>
    /// 关闭当前交互物体的交互
    /// </summary>
    protected void CloseCurrentModelInteraction()
    {
        string key = ModelCtrl.Instance.currClickModel.gameObject.name;
        Debug.Log($"关闭交互：<color=green>{key}</color>");

        ModelCtrl.Instance.Get(key).enable = false;
        ModelCtrl.Instance.Get(key).highlight.SetHighlighted(false);
    }

    protected void OpenCurrentModelInteraction()
    {
        string key = ModelCtrl.Instance.currClickModel.gameObject.name;
        Debug.Log($"开启交互：<color=green>{key}</color>");

        ModelCtrl.Instance.Get(key).enable = true;
        ModelCtrl.Instance.Get(key).highlight.SetHighlighted(false);
    }
}
