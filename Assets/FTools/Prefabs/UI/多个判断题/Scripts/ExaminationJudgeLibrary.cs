using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "JudgeInfo", menuName = "FDatas/JudgeInfo")]
[Serializable]
public class ExaminationJudgeLibrary : ScriptableObject
{
    public string groupName;
    public List<ExaminatinoJudgeInfo> examinationInfoList;

    [ContextMenu("Copy")]
    void Copy()
    {
        examinationInfoList = new List<ExaminatinoJudgeInfo>();
        string[][] form = FMethod.GetCopyExcelForm();
        foreach (var line in form)
        {
            examinationInfoList.Add(new ExaminatinoJudgeInfo(line[0], line[1] == "1", string.IsNullOrWhiteSpace(line[2]) ? 0 : int.Parse(line[2])));
        }
    }

    public void SetScore(int score)
    {
        for (int i = 0; i < examinationInfoList.Count; i++)
        {
            examinationInfoList[i].score = score;
        }
    }
}

[Serializable]
public class ExaminatinoJudgeInfo
{
    public string topic;
    public bool answer;
    public int score;

    public ExaminatinoJudgeInfo(string topic, bool answer, int score)
    {
        this. topic = topic;
        this.answer = answer;
        this.score = score;
    }
}
