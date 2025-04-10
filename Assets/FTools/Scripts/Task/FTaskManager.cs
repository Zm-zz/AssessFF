using Sirenix.OdinInspector;
using UnityEngine;

public class FTaskManager : SingletonPatternMonoBase<FTaskManager>
{
    [ReadOnly] public FTaskBase[] tasks;

    private void Update()
    {
        if (CheatingInstructions.isCheating)
        {

        }
    }

    public void InitTask()
    {
        tasks = GetComponentsInChildren<FTaskBase>(true);

        EventCenterManager.Broadcast(TaskCommand.Init_Before);
        EventCenterManager.Broadcast(TaskCommand.Init);
        foreach (var task in tasks)
        {
            task.InitTask();
        }
        EventCenterManager.Broadcast(TaskCommand.Init_After);
    }

    public void ResetTask()
    {
        foreach (var task in tasks)
        {
            task.ResetTask();
        }
    }

    public void StopTask()
    {
        EventCenterManager.Broadcast(TaskCommand.Stop_Before);
        EventCenterManager.Broadcast(TaskCommand.Stop);
        foreach (var task in tasks)
        {
            task.StopTask();
        }
        EventCenterManager.Broadcast(TaskCommand.Stop_After);

        FDatas.curIndex_流程序号 = -1;
    }

    public void EnterTask(int taskIndex)
    {
        if (GameManager.Instance.mode)
        {
            if (taskIndex <= FDatas.curIndex_流程序号)
            {
                return;
            }
        }

        TipManager.HideTips();
        foreach (var task in tasks)
        {
            if (task.taskIndex == FDatas.curIndex_流程序号)
            {
                task.ExitTask();
                break;
            }
        }
        foreach (var task in tasks)
        {
            if (task.taskIndex == taskIndex)
            {
                task.EnterTask();
                break;
            }
        }
    }

    public void EnterTask(string taskName)
    {
        /*if (GameManager.Instance.mode)
        {
            int taskIndex = 0;
            foreach (var task in tasks)
            {
                if (task.taskName == taskName)
                {
                    taskIndex = task.taskIndex;
                    break;
                }
            }
            if (taskIndex <= FDatas.curIndex_流程序号)
            {
                return;
            }
        }*/



        foreach (var task in tasks)
        {
            if (task.taskIndex == FDatas.curIndex_流程序号)
            {
                task.ExitTask();
                break;
            }
        }
        foreach (var task in tasks)
        {
            if (task.taskName == taskName)
            {
                task.EnterTask();
                break;
            }
        }
    }

    [Space]
    [SerializeField][BoxGroup("Skip Procedure")] private int index_任务序号;

    [Button]
    [BoxGroup("Skip Procedure")]
    void EnterTask()
    {
        EnterTask(index_任务序号);
    }
}
