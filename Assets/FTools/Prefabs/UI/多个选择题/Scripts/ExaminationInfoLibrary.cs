using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ExamInfo", menuName = "FDatas/ExamInfo")]
[Serializable]
public class ExaminationInfoLibrary : ScriptableObject
{
    public string groupName;
    public List<ExaminationInfoForm> examinationInfoList;

    [ContextMenu("Copy")]
    void Copy()
    {
        examinationInfoList = new List<ExaminationInfoForm>();
        string[][] form = FMethod.GetCopyExcelForm();
        foreach (var line in form)
        {
            examinationInfoList.Add(new ExaminationInfoForm(new ExaminationInfo(line[0], line[1].Split("\n"), int.Parse(line[2]))));
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
public class ExaminationInfoForm
{
    public string topic;
    public Option[] options;
    public int score;
    public bool isRadio;

    public ExaminationInfoForm(ExaminationInfo info)
    {
        topic = info.topic;
        options = new Option[info.options.Length];
        for (int i = 0; i < options.Length; i++)
        {
            options[i].option = info.options[i];
            options[i].isAnswer = info.answers[i];
        }
        score = info.score;
        isRadio = info.isRadio;
    }

    [Serializable]
    public class Option
    {
        public string option;
        public bool isAnswer;
    }
    public string[] GetOptions()
    {
        string[] options = new string[this.options.Length];
        for (int i = 0; i < this.options.Length; i++)
        {
            options[i] = this.options[i].option;
        }
        return options;
    }
    public bool[] GetAnswers()
    {
        bool[] answers = new bool[this.options.Length];
        for (int i = 0; i < this.options.Length; i++)
        {
            answers[i] = this.options[i].isAnswer;
        }
        return answers;
    }
}

[Serializable]
public class ExaminationInfo
{
    public string topic;      //标题
    public string[] options;  //选项描述
    public bool[] answers;    //答案
    public int score;         //选择题分值
    public bool isRadio;      //是否为单选
    public ExaminationInfo(string topic, string[] options, bool[] answers, int score = 0, bool isRadio = false)
    {
        this.topic = topic;
        this.options = options;
        this.answers = answers;
        this.score = score;
        this.isRadio = isRadio;
    }
    public ExaminationInfo(string topic, string[] options, int answers, int score = 0, bool isRadio = false)
    {
        this.topic = topic;
        this.options = options;
        this.answers = new bool[options.Length];
        while (answers > 0)
        {
            this.answers[(answers % 10) - 1] = true;
            answers /= 10;
        }
        this.score = score;
        this.isRadio = isRadio;
    }
    public ExaminationInfo(string topic, string[] options, string answerss, int score = 0, bool isRadio = false)
    {
        int length = 1;
        for (int i = 0; i < answerss.Length - 1; i++)
        {
            length *= 10;
        }
        int answers = 0;
        foreach (var i in answerss)
        {
            if (i == 'A' || i == 'a')
            {
                answers += 1 * length;
            }
            if (i == 'B' || i == 'b')
            {
                answers += 2 * length;
            }
            if (i == 'C' || i == 'c')
            {
                answers += 3 * length;
            }
            if (i == 'D' || i == 'd')
            {
                answers += 4 * length;
            }
            if (i == 'E' || i == 'e')
            {
                answers += 5 * length;
            }
            if (i == 'F' || i == 'f')
            {
                answers += 6 * length;
            }
            if (i == 'G' || i == 'g')
            {
                answers += 7 * length;
            }
            if (i == 'H' || i == 'h')
            {
                answers += 8 * length;
            }
            if (i == 'I' || i == 'i')
            {
                answers += 9 * length;
            }
            length /= 10;
        }
        this.topic = topic;
        this.options = options;
        this.answers = new bool[options.Length];
        while (answers > 0)
        {
            this.answers[(answers % 10) - 1] = true;
            answers /= 10;
        }
        this.score = score;
        this.isRadio = isRadio;
    }
}

