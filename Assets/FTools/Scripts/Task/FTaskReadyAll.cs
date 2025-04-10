using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ScriptExecutionOrder(5)]
public class FTaskReadyAll : MonoBehaviour
{
    [SerializeField] public List<TaskReadyAllData> readyList;

    private void Awake()
    {
        EventCenterManager.AddListener<int>(TaskCommand.Enter_Before, DoReady);
    }

    void DoReady(int index)
    {
        foreach (var ready in readyList)
        {
            ready.Invoke(index);
        }
    }
}

[Serializable]
public class TaskReadyAllData
{
    public int readyIndex;
    public TaskReadyType taskReadyType;
    public UnityEvent readyEvent;

    public TaskReadyAllData(int readyIndex, TaskReadyType taskReadyType, UnityEvent readyEvent)
    {
        this.readyIndex = readyIndex;
        this.taskReadyType = taskReadyType;
        this.readyEvent = readyEvent;
    }

    bool Satisfy(int index)
    {
        return taskReadyType switch
        {
            TaskReadyType.大于 => index > readyIndex,
            TaskReadyType.小于 => index < readyIndex,
            TaskReadyType.等于 => index == readyIndex,
            TaskReadyType.大于等于 => index >= readyIndex,
            TaskReadyType.小于等于 => index <= readyIndex,
            TaskReadyType.不等于 => index != readyIndex,
            _ => false,
        };
    }

    public void Invoke(int index)
    {
        if (Satisfy(index))
        {
            readyEvent?.Invoke();
        }
    }

    public enum TaskReadyType { 大于, 小于, 等于, 大于等于, 小于等于, 不等于 }
}
