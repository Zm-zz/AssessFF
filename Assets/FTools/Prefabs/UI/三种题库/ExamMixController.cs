using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ExamMixController : MonoBehaviour
{
    public ExaminationGroupController examinationGroup_��ѡ;
    public ExaminationGroupController examinationGroup_��ѡ;
    public ExaminationJudgeController examinationGroup_�ж�;
    public Button btn_�ύ��;
    public Button btn_���¿�����;
    public Button btn_������ҳ;

    public Button btn_���ش���;
    public Button btn_����ʵսģ��;

    public PopPanelConfirm panel_ȷ�ϵ���;

    private void Awake()
    {
        btn_�ύ��.onClick.AddListener(() =>
        {
            // panel_ȷ�ϵ���.PopPanel("�Ƿ��ύ�ɼ���", "��", "��", () => { OnConfirmSubmit(); }, () => { });
            EventCenterManager.Broadcast<UnityAction, UnityAction, UnityAction, string, string, string>
            (PopKey.PopUpTipWindow, () => { OnConfirmSubmit(); }, () => { }, () => { }, "�Ƿ��ύ�ɼ���", "��", "��");
            //OnConfirmSubmit();
        });

        //btn_���¿�����.onClick.AddListener(() =>
        //{
        //    examinationGroup_��ѡ.OnClick_���¿���();
        //    examinationGroup_��ѡ.OnClick_���¿���();
        //    examinationGroup_�ж�.OnClick_���¿���();
        //    btn_�ύ��.gameObject.SetActive(true);
        //    btn_������ҳ.gameObject.SetActive(false);
        //    btn_���¿�����.gameObject.SetActive(false);
        //});

        btn_���ش���.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitStep();
            GameManager.Instance.ShowHall(true);
        });

        btn_����ʵսģ��.onClick.AddListener(() =>
        {
            GameManager.Instance.ExitStep();
            GameManager.Instance.EnterStep(1, true);
        });
    }

    public void InitBut()
    {
        btn_�ύ��.gameObject.SetActive(true);
        btn_������ҳ.gameObject.SetActive(false);
        btn_���¿�����.gameObject.SetActive(false);
    }

    /// <summary>
    /// ȷ���ύ
    /// </summary>
    private void OnConfirmSubmit()
    {
        examinationGroup_��ѡ.OnClick_�ύ();
        examinationGroup_��ѡ.OnClick_�ύ();
        examinationGroup_�ж�.OnClick_�ύ();
        btn_�ύ��.gameObject.SetActive(false);
        btn_������ҳ.gameObject.SetActive(true);
        btn_���¿�����.gameObject.SetActive(true);

        // GameManager.Instance.kqdx.CalculateScore();

        //if (GameManager.Instance.kqdx.correctCount < 7)
        //{
        //    btn_����ʵսģ��.interactable = false;
        //}
        //else
        //{
        //    btn_����ʵսģ��.interactable = true;
        //    GameManager.Instance.SetStepLock(1, true);
        //}

        ScoreController.Instance.SubmitStep1Score();
    }

    public void SetExam(List<ExaminationInfo> list_��ѡ��, List<ExaminationInfo> list_��ѡ��, List<ExaminatinoJudgeInfo> list_�ж���)
    {
        examinationGroup_��ѡ.SetExamination(list_��ѡ��);
        examinationGroup_��ѡ.SetExamination(list_��ѡ��);
        examinationGroup_�ж�.SetExamination(list_�ж���);
    }

    public int GetScore()
    {
        return examinationGroup_��ѡ.lastScore + examinationGroup_��ѡ.lastScore + examinationGroup_�ж�.lastScore;
    }

    [InspectorButton()]
    void SetExam()
    {
        examinationGroup_��ѡ.SetExamination("ѡ������");
        examinationGroup_��ѡ.SetExamination("ѡ������");
        examinationGroup_�ж�.SetExamination("�ж�����");
    }
}
