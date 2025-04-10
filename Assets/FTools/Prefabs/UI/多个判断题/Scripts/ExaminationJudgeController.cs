using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationJudgeController : MonoBehaviour
{
    public List<ExaminationJudgeLibrary> examinationGroupInfoList;

    //�����ȡ
    bool hasScore;
    bool isRandom_����;
    int examCount;
    List<ExaminatinoJudgeInfo> curInfoList;

    [SerializeField] private Sprite sprite_Ĭ��;
    [SerializeField] private Sprite sprite_ѡ��;
    [SerializeField] private Sprite sprite_��ȷ;
    [SerializeField] private Sprite sprite_����;
    [SerializeField] private Sprite sprite_©ѡ;

    [Space]
    [SerializeField] private GameObject panel_���������;
    [SerializeField] private GameObject prefab_��Ŀ��;
    [SerializeField] private Transform trans_��Ŀ��;

    List<ExaminatinoJudgeInfo> examinationInfoList;
    List<GameObject> examinationObjList;

    [HideInInspector] public int lastScore;

    private void Awake()
    {
        examinationInfoList = new List<ExaminatinoJudgeInfo>();
        examinationObjList = new List<GameObject>();
    }

    public void SetActive(bool b)
    {
        panel_���������.SetActive(b);
    }

    /// <summary>
    /// ���ÿ���
    /// </summary>
    /// <param name="list">�����б�</param>
    /// <param name="hasScore">�Ƿ����÷���</param>
    /// <param name="isRandom_����">�������</param>
    /// <param name="isRandom_ѡ��">ѡ�����</param>
    /// <param name="examCount">���б��г�ȡ��������</param>
    public void SetExamination(List<ExaminatinoJudgeInfo> list, bool hasScore = false, bool isRandom_���� = false, int examCount = -1)
    {
        this.hasScore = hasScore;
        this.isRandom_���� = isRandom_����;
        this.examCount = examCount;
        curInfoList = new List<ExaminatinoJudgeInfo>(list);
        if (examCount > list.Count)
        {
            Debug.LogError("�����ȡ����");
            examCount = -1;
        }
        if (examCount == -1) examCount = list.Count;
        SetActive(true);
        foreach (var obj in examinationObjList)
        {
            Destroy(obj);
        }
        examinationObjList.Clear();
        examinationInfoList.Clear();
        examinationInfoList = list;
        examinationInfoList = isRandom_���� ? FMethod.RandomList(list).GetRange(0, examCount) : list.GetRange(0, examCount);
        foreach (var examination in examinationInfoList)
        {
            GameObject exam = Instantiate(prefab_��Ŀ��, trans_��Ŀ��);
            exam.transform.Find("��Ŀ����").GetComponent<Text>().text = examination.topic;
            exam.transform.Find("��ȷ").GetComponent<Toggle>().interactable = true;
            exam.transform.Find("����").GetComponent<Toggle>().interactable = true;
            exam.transform.Find("��ȷ").GetComponent<Image>().sprite = sprite_Ĭ��;
            exam.transform.Find("��ȷ").GetComponent<ToggleValueChange>().sprite_on = sprite_ѡ��;
            exam.transform.Find("��ȷ").GetComponent<ToggleValueChange>().sprite_off = sprite_Ĭ��;
            exam.transform.Find("����").GetComponent<Image>().sprite = sprite_Ĭ��;
            exam.transform.Find("����").GetComponent<ToggleValueChange>().sprite_on = sprite_ѡ��;
            exam.transform.Find("����").GetComponent<ToggleValueChange>().sprite_off = sprite_Ĭ��;
            exam.transform.Find("��ȷ").GetComponentInChildren<Text>().enabled = true;
            exam.transform.Find("����").GetComponentInChildren<Text>().enabled = true;
            examinationObjList.Add(exam);
        }
    }

    public void SetExamination(string examGroupName, bool hasScore = false, bool isRandom_���� = false, int examCount = -1)
    {
        SetExamination(GetExaminations(examGroupName), hasScore, isRandom_����, examCount);
    }
    public List<ExaminatinoJudgeInfo> GetExaminations(string groupName)
    {
        List<ExaminatinoJudgeInfo> examinationInfoList = new List<ExaminatinoJudgeInfo>();
        foreach (var examinationGroupInfo in examinationGroupInfoList)
        {
            if (examinationGroupInfo.groupName == groupName)
            {
                foreach (var examinationInfoForm in examinationGroupInfo.examinationInfoList)
                {
                    examinationInfoList.Add(new ExaminatinoJudgeInfo(examinationInfoForm.topic, examinationInfoForm.answer, examinationInfoForm.score));
                }
                return examinationInfoList;
            }
        }
        Debug.LogWarning($"δ�ҵ���Ϊ {groupName} �Ŀ�����");
        return default;
    }

    void Correct()
    {
        int totalScore = 0;
        int correctCount = 0;
        int wrongCount = 0;
        for (int i = 0; i < examinationInfoList.Count; i++)
        {
            bool isCorrect = true;
            ExaminatinoJudgeInfo info = examinationInfoList[i];
            GameObject obj = examinationObjList[i];
            if (info.answer)
            {
                if (obj.transform.Find("��ȷ").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("��ȷ").GetComponent<Image>().sprite = sprite_��ȷ;
                    obj.transform.Find("��ȷ").GetComponentInChildren<Text>().enabled = false;
                }
                else
                {
                    obj.transform.Find("��ȷ").GetComponent<Image>().sprite = sprite_©ѡ;
                    isCorrect = false;
                }
                if (obj.transform.Find("����").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("����").GetComponent<Image>().sprite = sprite_����;
                    obj.transform.Find("����").GetComponentInChildren<Text>().enabled = false;
                    isCorrect = false;
                }
            }
            else
            {
                if (obj.transform.Find("��ȷ").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("��ȷ").GetComponent<Image>().sprite = sprite_����;
                    obj.transform.Find("��ȷ").GetComponentInChildren<Text>().enabled = false;
                    isCorrect = false;
                }
                if (obj.transform.Find("����").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("����").GetComponent<Image>().sprite = sprite_��ȷ;
                    obj.transform.Find("����").GetComponentInChildren<Text>().enabled = false;
                }
                else
                {
                    obj.transform.Find("����").GetComponent<Image>().sprite = sprite_©ѡ;
                    isCorrect = false;
                }
            }
            if(isCorrect)
            {
                totalScore += info.score;
                    correctCount++;
            }
            else
            {
                wrongCount++;
            }
            obj.transform.Find("��ȷ").GetComponent<Toggle>().interactable = false;
            obj.transform.Find("����").GetComponent<Toggle>().interactable = false;

        }
        lastScore = totalScore;
    }

    public void OnClick_�ύ()
    {
        Correct();
    }
    void OnClick_������ҳ()
    {
        SetActive(false);
    }
    public void OnClick_���¿���()
    {
        List<ExaminatinoJudgeInfo> list = new List<ExaminatinoJudgeInfo>(curInfoList);
        SetExamination(list, hasScore, isRandom_����, examCount);
    }
}
