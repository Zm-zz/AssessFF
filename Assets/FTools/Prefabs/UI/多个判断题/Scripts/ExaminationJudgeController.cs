using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationJudgeController : MonoBehaviour
{
    public List<ExaminationJudgeLibrary> examinationGroupInfoList;

    //从组获取
    bool hasScore;
    bool isRandom_考题;
    int examCount;
    List<ExaminatinoJudgeInfo> curInfoList;

    [SerializeField] private Sprite sprite_默认;
    [SerializeField] private Sprite sprite_选中;
    [SerializeField] private Sprite sprite_正确;
    [SerializeField] private Sprite sprite_错误;
    [SerializeField] private Sprite sprite_漏选;

    [Space]
    [SerializeField] private GameObject panel_考题总面板;
    [SerializeField] private GameObject prefab_题目项;
    [SerializeField] private Transform trans_题目父;

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
        panel_考题总面板.SetActive(b);
    }

    /// <summary>
    /// 设置考题
    /// </summary>
    /// <param name="list">考题列表</param>
    /// <param name="hasScore">是否设置分数</param>
    /// <param name="isRandom_考题">考题随机</param>
    /// <param name="isRandom_选项">选项随机</param>
    /// <param name="examCount">从列表中抽取考题数量</param>
    public void SetExamination(List<ExaminatinoJudgeInfo> list, bool hasScore = false, bool isRandom_考题 = false, int examCount = -1)
    {
        this.hasScore = hasScore;
        this.isRandom_考题 = isRandom_考题;
        this.examCount = examCount;
        curInfoList = new List<ExaminatinoJudgeInfo>(list);
        if (examCount > list.Count)
        {
            Debug.LogError("考题获取过多");
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
        examinationInfoList = isRandom_考题 ? FMethod.RandomList(list).GetRange(0, examCount) : list.GetRange(0, examCount);
        foreach (var examination in examinationInfoList)
        {
            GameObject exam = Instantiate(prefab_题目项, trans_题目父);
            exam.transform.Find("题目描述").GetComponent<Text>().text = examination.topic;
            exam.transform.Find("正确").GetComponent<Toggle>().interactable = true;
            exam.transform.Find("错误").GetComponent<Toggle>().interactable = true;
            exam.transform.Find("正确").GetComponent<Image>().sprite = sprite_默认;
            exam.transform.Find("正确").GetComponent<ToggleValueChange>().sprite_on = sprite_选中;
            exam.transform.Find("正确").GetComponent<ToggleValueChange>().sprite_off = sprite_默认;
            exam.transform.Find("错误").GetComponent<Image>().sprite = sprite_默认;
            exam.transform.Find("错误").GetComponent<ToggleValueChange>().sprite_on = sprite_选中;
            exam.transform.Find("错误").GetComponent<ToggleValueChange>().sprite_off = sprite_默认;
            exam.transform.Find("正确").GetComponentInChildren<Text>().enabled = true;
            exam.transform.Find("错误").GetComponentInChildren<Text>().enabled = true;
            examinationObjList.Add(exam);
        }
    }

    public void SetExamination(string examGroupName, bool hasScore = false, bool isRandom_考题 = false, int examCount = -1)
    {
        SetExamination(GetExaminations(examGroupName), hasScore, isRandom_考题, examCount);
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
        Debug.LogWarning($"未找到名为 {groupName} 的考题组");
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
                if (obj.transform.Find("正确").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("正确").GetComponent<Image>().sprite = sprite_正确;
                    obj.transform.Find("正确").GetComponentInChildren<Text>().enabled = false;
                }
                else
                {
                    obj.transform.Find("正确").GetComponent<Image>().sprite = sprite_漏选;
                    isCorrect = false;
                }
                if (obj.transform.Find("错误").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("错误").GetComponent<Image>().sprite = sprite_错误;
                    obj.transform.Find("错误").GetComponentInChildren<Text>().enabled = false;
                    isCorrect = false;
                }
            }
            else
            {
                if (obj.transform.Find("正确").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("正确").GetComponent<Image>().sprite = sprite_错误;
                    obj.transform.Find("正确").GetComponentInChildren<Text>().enabled = false;
                    isCorrect = false;
                }
                if (obj.transform.Find("错误").GetComponent<Toggle>().isOn)
                {
                    obj.transform.Find("错误").GetComponent<Image>().sprite = sprite_正确;
                    obj.transform.Find("错误").GetComponentInChildren<Text>().enabled = false;
                }
                else
                {
                    obj.transform.Find("错误").GetComponent<Image>().sprite = sprite_漏选;
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
            obj.transform.Find("正确").GetComponent<Toggle>().interactable = false;
            obj.transform.Find("错误").GetComponent<Toggle>().interactable = false;

        }
        lastScore = totalScore;
    }

    public void OnClick_提交()
    {
        Correct();
    }
    void OnClick_返回首页()
    {
        SetActive(false);
    }
    public void OnClick_重新考核()
    {
        List<ExaminatinoJudgeInfo> list = new List<ExaminatinoJudgeInfo>(curInfoList);
        SetExamination(list, hasScore, isRandom_考题, examCount);
    }
}
