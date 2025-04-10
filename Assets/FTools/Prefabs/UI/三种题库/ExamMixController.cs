using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExamMixController : MonoBehaviour
{
    public ExaminationGroupController examinationGroup_单选;
    public ExaminationGroupController examinationGroup_多选;
    public ExaminationJudgeController examinationGroup_判断;
    public Button btn_提交总;
    public Button btn_重新考核总;
    public Button btn_返回首页;

    public Button btn_返回大厅;
    public Button btn_继续实战模块;

    public PopPanelConfirm panel_确认弹窗;

    private void Awake()
    {
        btn_提交总.onClick.AddListener(() =>
        {
            // panel_确认弹窗.PopPanel("是否提交成绩？", "是", "否", () => { OnConfirmSubmit(); }, () => { });
            EventCenterManager.Broadcast<UnityAction, UnityAction, UnityAction, string, string, string>
            (PopKey.PopUpTipWindow, () => { OnConfirmSubmit(); }, () => { }, () => { }, "是否提交成绩？", "是", "否");
            //OnConfirmSubmit();
        });

        //btn_重新考核总.onClick.AddListener(() =>
        //{
        //    examinationGroup_单选.OnClick_重新考核();
        //    examinationGroup_多选.OnClick_重新考核();
        //    examinationGroup_判断.OnClick_重新考核();
        //    btn_提交总.gameObject.SetActive(true);
        //    btn_返回首页.gameObject.SetActive(false);
        //    btn_重新考核总.gameObject.SetActive(false);
        //});

        btn_返回大厅.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitStep();
            GameManager.Instance.ShowHall(true);
        });

        btn_继续实战模块.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitStep();
            GameManager.Instance.EnterStep(1, true);
        });
    }

    public void InitBut()
    {
        btn_提交总.gameObject.SetActive(true);
        btn_返回首页.gameObject.SetActive(false);
        btn_重新考核总.gameObject.SetActive(false);
    }

    /// <summary>
    /// 确认提交
    /// </summary>
    private void OnConfirmSubmit()
    {
        examinationGroup_单选.OnClick_提交();
        examinationGroup_多选.OnClick_提交();
        examinationGroup_判断.OnClick_提交();
        btn_提交总.gameObject.SetActive(false);
        btn_返回首页.gameObject.SetActive(true);
        btn_重新考核总.gameObject.SetActive(true);

        // GameManager.Instance.kqdx.CalculateScore();

        //if (GameManager.Instance.kqdx.correctCount < 7)
        //{
        //    btn_继续实战模块.interactable = false;
        //}
        //else
        //{
        //    btn_继续实战模块.interactable = true;
        //    GameManager.Instance.SetStepLock(1, true);
        //}

        ScoreController.Instance.SubmitStep1Score();
    }

    public void SetExam(List<ExaminationInfo> list_单选题, List<ExaminationInfo> list_多选题, List<ExaminatinoJudgeInfo> list_判断题)
    {
        examinationGroup_单选.SetExamination(list_单选题);
        examinationGroup_多选.SetExamination(list_多选题);
        examinationGroup_判断.SetExamination(list_判断题);
    }

    public int GetScore()
    {
        return examinationGroup_单选.lastScore + examinationGroup_多选.lastScore + examinationGroup_判断.lastScore;
    }

    [InspectorButton()]
    void SetExam()
    {
        examinationGroup_单选.SetExamination("选择题组");
        examinationGroup_多选.SetExamination("选择题组");
        examinationGroup_判断.SetExamination("判断题组");
    }
}
