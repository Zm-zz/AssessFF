using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonPatternMonoBase<GameManager>
{
    [Header("����")]
    public bool test;

    [Space()]
    [Header("Backend")]
    public string appID = "999";

    [Space()]
    [Header("false->ѵ�� -- true->����")]
    public bool mode;

    [Space()]
    [Header("�û�")]
    public string UserName;
    public string UserSchool;

    [Space()]
    [Header("��ҳ")]
    public Transform home;

    [Space()]
    [Header("����")]
    public Transform hall;
    public Text txt_hall;

    [Space()]
    [Header("�׶���")]
    public List<bool> stepLock;

    [Space()]
    [Header("��׶�")]
    public int currentStep = -1;

    [Space()]
    [Header("���������")]
    public FTaskManager taskMgr;

    [Header("��������")]
    public Transform uiParent;
    public List<MenuToggleBase> allTask;

    [Space()]
    [Header("�˵�")]
    public ToggleManager togMgr;
    public Transform togContent;
    [Header("Ԥ����")]
    public SingleToggle par_Menu;
    public SpreadedToggleManager subPar_Menu;
    public SingleSpreadedToggle sub_Menu;

    [HideInInspector]
    public int startTime; // ��ʼʱ��

    private void Start()
    {
        Backend.Instance.Init(InitBackend());

        ShowHome(true);

        Init();
    }

    private BackendData InitBackend()
    {
        BackendData data = new BackendData();
        data.IsStandalone = false;
        data.AppID = $"S-000{appID}";
        data.AppProtocol = $"UGION000{appID}";
        data.AppVersion = "1.0";

        return data;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.T))
        {
            ScoreController.Instance.ShowScore();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ScoreController.Instance.CloseScore();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            EventCenterManager.Broadcast(PopKey.PopUpRobotTip, "��������ʾ����");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            EventCenterManager.Broadcast(PopKey.PopDownRobotTip);
        }
#endif

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    for (int i = 1; i < stepLock.Count; i++)
        //    {
        //        SetStepLock(i, true);
        //    }
        //}
    }

    public void Init()
    {
        StepStateReset();
        taskMgr.InitTask();

        // AllTaskInit();

        ModelCtrl.Instance.Init();
        Robot.Instance.Initialize();
        Robot.Instance.HideImmediate();
        //Robot.Instance.gameObject.SetActive(false);
        Robot.Instance.transform.localScale = Vector3.zero;

        // �˵���ʼ�������ƶ���֮���ʼ���������ƶ���֮ǰ
        TaskInit();

        if (test)
        {
            for (int i = 0; i < stepLock.Count; i++)
            {
                stepLock[i] = true;
            }

            StepStateReset();
        }
    }

    /// <summary>
    /// ��������˵���ʼ��
    /// </summary>
    private void TaskInit()
    {
        AutoCreateTMenu(transform.GetChild(0));
        togMgr.Init();
        allTask = uiParent.GetComponentsInChildren<MenuToggleBase>(true).ToList();
    }

    private void AutoCreateTMenu(Transform parent)
    {
        void GetSubMenuRecursively(Transform menu, bool isParent)
        {
            if (isParent) return;

            if (menu.transform.childCount != 0)
            {
                SingleToggle single = Instantiate(par_Menu, togContent);
                single.Toggle.group = togMgr.GetComponent<ToggleGroup>();
                single.gameObject.name = menu.name;
                single.Label.text = menu.name;
                SpreadedToggleManager spreadMgr = Instantiate(subPar_Menu, togContent);
                spreadMgr.gameObject.name = $"Sub_{menu.name}";
                single.hasSpread = true;
                single.spreadMgr = spreadMgr;
                spreadMgr.gameObject.SetActive(false);

                for (int i = 0; i < menu.transform.childCount; i++)
                {
                    FTaskBase task = menu.GetChild(i).GetComponent<FTaskBase>();
                    if (task)
                    {
                        SingleSpreadedToggle singleSpread = Instantiate(sub_Menu, spreadMgr.transform);
                        singleSpread.gameObject.name = $"{task.taskIndex}_{task.name}";
                        singleSpread.Toggle.group = spreadMgr.GetComponent<ToggleGroup>();
                        singleSpread.taskIndex = task.taskIndex;
                        singleSpread.taskName = task.taskName;
                        singleSpread.Label.text = task.gameObject.name;
                    }
                }
            }
            else
            {
                SingleToggle single = Instantiate(par_Menu, togContent);
                single.Toggle.group = togMgr.GetComponent<ToggleGroup>();
                single.hasSpread = false;
                single.spreadMgr = null;
                FTaskBase task = menu.GetComponent<FTaskBase>();
                single.gameObject.name = $"{task.taskIndex}_{task.name}";
                single.taskIndex = task.taskIndex;
                single.taskName = task.taskName;
                single.Label.text = menu.name;
            }
        }

        for (int i = 0; i < parent.childCount; i++)
        {
            GetSubMenuRecursively(parent.GetChild(i), false);
        }
    }

    /// <summary>
    /// false->ѵ�� -- true->����
    /// </summary>
    /// <param name="mode"></param>
    public void SetMode(bool mode)
    {
        this.mode = mode;

        txt_hall.text = mode ? "��ʼ����" : "��ʼѵ��";

        if (test) return;

        if (mode)
        {
            for (int i = 1; i < stepLock.Count; i++)
            {
                SetStepLock(i, false);
            }
        }
    }

    public void SetStepLock(int index, bool isLock)
    {
        stepLock[index] = isLock;
        StepStateReset();
    }

    public void ShowHome(bool show)
    {
        ShowHall(false);
        home.gameObject.SetActive(show);
        Robot.Instance.HideImmediate();
        //Robot.Instance.gameObject.SetActive(!show);
        Robot.Instance.transform.localScale = show ? Vector3.one : Vector3.one;
    }

    public void ShowHall(bool show)
    {
        hall.gameObject.SetActive(show);
        Robot.Instance.HideImmediate();
        // Robot.Instance.gameObject.SetActive(!show);
        Robot.Instance.transform.localScale = show ? Vector3.one : Vector3.one;
    }

    private void AllTaskInit()
    {
        allTask = uiParent.GetComponentsInChildren<MenuToggleBase>(true).ToList();

        int taskCount = 0;

        try
        {
            for (int i = 0; i < allTask.Count; i++)
            {
                if (allTask[i].hasSpread) continue;

                allTask[i].gameObject.name = taskMgr.tasks[taskCount].taskName;
                allTask[i].taskIndex = taskMgr.tasks[taskCount].taskIndex;
                allTask[i].taskName = taskMgr.tasks[taskCount].taskName;
                taskCount++;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }

    }

    public void NextTask()
    {
        EnterTask(FDatas.curIndex_������� + 1);
    }

    public void EnterTask(int index)
    {
        #region ����
        //foreach (var task in allTask)
        //{
        //    if (task.taskIndex == 0)
        //        continue;

        //    if (task.taskIndex == index)
        //    {
        //        task.Toggle.isOn = true;
        //    }
        //}
        #endregion

        if (mode)
        {
            // if (index <= FDatas.curIndex_�������) return;
        }

        for (int i = 0; i < allTask.Count; i++)
        {
            if (allTask[i].taskIndex == 0)
                continue;

            if (allTask[i].taskIndex == index)
            {
                // �ò�����Ϸ����Ϊnull��֤���Ǵ����е�С����
                if (!allTask[i].gameObject.activeInHierarchy)
                {
                    ToggleManager tg = allTask[i].GetComponentInParent<ToggleManager>();
                    tg.SelectToggle(tg.lastSelectIndex + 1);
                }
                else
                {
                    allTask[i].Toggle.isOn = true;
                }
            }
        }
    }

    public void ExitTask()
    {
        taskMgr.StopTask();
    }

    public void EnterTask(string name)
    {
        foreach (var task in allTask)
        {
            if (task.taskName == "")
                continue;

            if (task.taskName == name)
            {
                task.Toggle.isOn = true;
            }
        }
    }

    /// <summary>
    /// ����׶�״̬
    /// </summary>
    public void StepStateReset()
    {
        //foreach (var step in stepControls)
        //{
        //    step.ResetState();
        //}
    }

    /// <summary>
    /// ����ѡ��׶�
    /// </summary>
    public void EnterStep()
    {
        Backend.Instance.BeginTrain(!mode);
        startTime = ShowTime();
        //EnterStep(stepMgr.currentSelect);
    }

    /// <summary>
    /// �Զ���׶�
    /// </summary>
    /// <param name="stepIndex"></param>
    /// <param name="insideInvoke">�Ƿ�����λ�õ��ã����Ǵ�����</param>
    public void EnterStep(int stepIndex, bool insideInvoke = false)
    {
        Backend.Instance.BeginTrain(!mode);

        //if (insideInvoke)
        //{
        //    stepMgr.currentSelect = stepIndex;
        //    stepControls[stepIndex].currentSelect = true;
        //}

        //if (stepMgr.currentSelect == -1)
        //    return;

        //if (insideInvoke)
        //{
        //    stepMgr.currentSelect = stepIndex;
        //    stepControls[stepIndex].currentSelect = true;
        //    // currentStep = stepIndex;
        //}

        //// ֮ǰѡ�еĽ׶Σ�֮����ȡ��ѡ����
        //if (stepIndex != -1 && !stepControls[stepIndex].currentSelect)
        //{
        //    Debug.Log($"���׶�<color=red>  {stepIndex}  </color>�Ƿ��ѡ");
        //    return;
        //}

        ShowTime();
        ShowHall(false);
        ShowHome(false);

        if (currentStep != -1 && currentStep != stepIndex)
        {
            //processSteps[currentStep].OnExit();
        }

        currentStep = stepIndex;
        // stepMgr.currentSelect = stepIndex;
        Debug.Log($"����׶Σ�<color=blue>{currentStep}</color>");
        //processSteps[stepIndex].OnEnter();

        // �رմ���
        //stepMgr.gameObject.SetActive(false);
    }

    /// <summary>
    /// �˳��׶�
    /// </summary>
    public void ExitStep()
    {
        //stepControls[currentStep].ResetState();
        // processSteps[currentStep].OnExit();
        //stepMgr.currentSelect = -1;
        currentStep = -1;

    }

    public int ShowTime()
    {
        int time = (int)Time.time;

        Debug.Log($"<color=yellow>����ʱ��:{ConvertSecondsToTime(time)}</color>");

        return time;
    }

    public string ConvertSecondsToTime(int totalSeconds)
    {
        int hours = (int)Mathf.Floor(totalSeconds / 3600);
        int minutes = (int)Mathf.Floor((totalSeconds % 3600) / 60);
        int seconds = totalSeconds % 60;

        return $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}

public enum EventKey
{
    ��ǰ��ѧ_����ָ����ʼ��,
    ��ǰ��ѧ_��ѧ��ʼ��,
    ʶ��Ԥ����_��ʼ��,
}
