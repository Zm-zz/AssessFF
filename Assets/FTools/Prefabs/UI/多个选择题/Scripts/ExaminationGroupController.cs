using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationGroupController : MonoBehaviour
{
    public List<ExaminationInfoLibrary> examinationGroupInfoList;
    
    //从组获取
    bool hasScore;
    bool isRandom_考题;
    bool isRandom_选项;
    int examCount;
    List<ExaminationInfo> curInfoList;
    [Space]
    [SerializeField] private Sprite sprite_题目正确;
    [SerializeField] private Sprite sprite_题目错误;
    [SerializeField] private Sprite sprite_单选默认;
    [SerializeField] private Sprite sprite_单选移入;
    [SerializeField] private Sprite sprite_单选选中;
    [SerializeField] private Sprite sprite_单选正确;
    [SerializeField] private Sprite sprite_单选错误;
    [SerializeField] private Sprite sprite_单选漏选;
    [SerializeField] private Sprite sprite_多选默认;
    [SerializeField] private Sprite sprite_多选移入;
    [SerializeField] private Sprite sprite_多选选中;
    [SerializeField] private Sprite sprite_多选正确;
    [SerializeField] private Sprite sprite_多选错误;
    [SerializeField] private Sprite sprite_多选漏选;
    [SerializeField] private Color color_正确;
    [SerializeField] private Color color_错误;
    [SerializeField] private Color color_漏选;
    [SerializeField] private Color color_默认;
    [Space]
    [SerializeField] private GameObject panel_考题总面板;
    [SerializeField] private Text text_总题目计数;
    [SerializeField] private Text text_正确题目计数;
    [SerializeField] private Text text_错误题目计数;

    [SerializeField] private GameObject prefab_考题项;
    [SerializeField] private GameObject prefab_考题选项;
    [SerializeField] private Transform trans_考题父;

    [SerializeField] private Image img_总分显示框;
    [SerializeField] private Sprite sprite_及格;
    [SerializeField] private Sprite sprite_不及格;
    [SerializeField] private Text text_总分;

    [SerializeField] private Button btn_提交按钮;
    [SerializeField] private Button btn_重新考核按钮;
    [SerializeField] private Button btn_返回首页按钮;

    List<ExaminationInfo> examinationInfoList;
    List<GameObject> examinationObjList;

    [HideInInspector] public int lastScore;

    private void Awake()
    {
        examinationInfoList = new List<ExaminationInfo>();
        examinationObjList = new List<GameObject>();
        btn_提交按钮.onClick.AddListener(OnClick_提交);
        btn_返回首页按钮.onClick.AddListener(OnClick_返回首页);
        btn_重新考核按钮.onClick.AddListener(OnClick_重新考核);
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
    public void SetExamination(List<ExaminationInfo> list, bool hasScore = false, bool isRandom_考题 = false, bool isRandom_选项 = false, int examCount = -1)
    {
        this.hasScore = hasScore;
        this.isRandom_考题 = isRandom_考题;
        this.isRandom_选项 = isRandom_选项;
        this.examCount = examCount;
        curInfoList = new List<ExaminationInfo>(list);
        if (examCount > list.Count)
        {
            Debug.LogError("考题获取过多");
            examCount = -1;
        }
        if (examCount == -1) examCount = list.Count;
        if (isRandom_选项)
        {
            List<ExaminationInfo> eInfo = new List<ExaminationInfo>();
            for (int index = 0; index < list.Count; index++)
            {
                ExaminationInfo eIndex = list[index];
                string[] options = new string[eIndex.options.Length];
                bool[] answers = new bool[eIndex.answers.Length];
                //打乱选项和答案
                List<int> iIndexlist = new List<int>();
                for (int i = 0; i < eIndex.options.Length; i++)
                {
                    int index2 = UnityEngine.Random.Range(0, eIndex.options.Length);
                    while (iIndexlist.Contains(index2))
                    {
                        index2 = UnityEngine.Random.Range(0, eIndex.options.Length);
                    }
                    iIndexlist.Add(index2);
                    options[i] = eIndex.options[index2];
                    answers[i] = eIndex.answers[index2];
                }
                ExaminationInfo ee = new ExaminationInfo(eIndex.topic, options, answers, eIndex.score, eIndex.isRadio);
                eInfo.Add(ee);
            }
            list = eInfo;
        }
        SetActive(true);
        btn_提交按钮.gameObject.SetActive(true);
        btn_返回首页按钮.gameObject.SetActive(false);
        btn_重新考核按钮.gameObject.SetActive(false);
        foreach (var obj in examinationObjList)
        {
            Destroy(obj);
        }
        examinationObjList.Clear();
        examinationInfoList.Clear();
        examinationInfoList = list;
        examinationInfoList = isRandom_考题 ? FMethod.RandomList(list).GetRange(0, examCount) : list.GetRange(0, examCount);

        img_总分显示框.gameObject.SetActive(false);
        foreach (var examination in examinationInfoList)
        {
            GameObject exam = Instantiate(prefab_考题项, trans_考题父);
            exam.transform.Find("考题项/标题").GetComponent<Text>().text = examination.topic;
            Transform trans_选项父 = exam.transform.Find("考题项/选项组");
            for (int i = 0; i < trans_选项父.childCount; i++)
            {
                Destroy(trans_选项父.GetChild(i).gameObject);
            }
            for (int i = 0; i < examination.options.Length; i++)
            {
                GameObject item = Instantiate(prefab_考题选项, trans_选项父);
                item.transform.GetComponentInChildren<Text>().text = examination.options[i];
                item.GetComponentInChildren<Text>().color = Color.black;
                item.GetComponent<Toggle>().interactable = true;
                if (examination.isRadio)
                {
                    item.GetComponent<Toggle>().group = trans_选项父.GetComponent<ToggleGroup>();
                    item.GetComponentInChildren<Image>().sprite = sprite_单选默认;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_off = sprite_单选默认;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_on = sprite_单选选中;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_highlight_off = sprite_单选移入;
                }
                else
                {
                    item.GetComponent<Toggle>().group = null;
                    item.GetComponentInChildren<Image>().sprite = sprite_多选默认;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_off = sprite_多选默认;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_on = sprite_多选选中;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_highlight_off = sprite_多选移入;
                }
            }
            exam.transform.Find("正误显示").gameObject.SetActive(false);
            examinationObjList.Add(exam);
        }
        text_总题目计数.gameObject.SetActive(true);
        text_正确题目计数.gameObject.SetActive(false);
        text_错误题目计数.gameObject.SetActive(false);
        text_总题目计数.text = $"共{examinationInfoList.Count}道";
    }
    public void SetExamination(string examGroupName, bool hasScore = false, bool isRandom_考题 = false, bool isRandom_选项 = false, int examCount = -1)
    {
        SetExamination(GetExaminations(examGroupName), hasScore, isRandom_考题, isRandom_选项, examCount);
    }
    public List<ExaminationInfo> GetExaminations(string groupName)
    {
        List<ExaminationInfo> examinationInfoList = new List<ExaminationInfo>();
        foreach (var examinationGroupInfo in examinationGroupInfoList)
        {
            if (examinationGroupInfo.groupName == groupName)
            {
                foreach (var examinationInfoForm in examinationGroupInfo.examinationInfoList)
                {
                    examinationInfoList.Add(new ExaminationInfo(examinationInfoForm.topic, examinationInfoForm.GetOptions(), examinationInfoForm.GetAnswers(), examinationInfoForm.score, examinationInfoForm.isRadio));
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
            ExaminationInfo info = examinationInfoList[i];
            GameObject obj = examinationObjList[i];
            for (int j = 0; j < info.options.Length; j++)
            {

                GameObject item = obj.transform.Find("考题项/选项组").GetChild(j).gameObject;
                if (info.answers[j])
                {
                    if (item.GetComponent<Toggle>().isOn)
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_单选正确;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_多选正确;
                        }
                        item.GetComponentInChildren<Text>().color = color_正确;
                    }
                    else
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_单选漏选;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_多选漏选;
                        }
                        item.GetComponentInChildren<Text>().color = color_漏选;
                        isCorrect = false;
                    }
                }
                else
                {
                    if (item.GetComponent<Toggle>().isOn)
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_单选错误;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_多选错误;
                        }
                        item.GetComponentInChildren<Image>().sprite = sprite_单选错误;
                        item.GetComponentInChildren<Text>().color = color_错误;
                        isCorrect = false;
                    }
                    else
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_单选默认;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_多选默认;
                        }
                        item.GetComponentInChildren<Text>().color = color_默认;
                    }
                }
                item.GetComponent<Toggle>().interactable = false;
            }
            obj.transform.Find("正误显示").gameObject.SetActive(true);
            if (isCorrect)
            {
                obj.transform.Find("正误显示").GetComponent<Image>().sprite = sprite_题目正确;
                totalScore += info.score;
                correctCount++;
            }
            else
            {
                obj.transform.Find("正误显示").GetComponent<Image>().sprite = sprite_题目错误;
                wrongCount++;
            }
            text_总题目计数.gameObject.SetActive(false);
            text_正确题目计数.gameObject.SetActive(true);
            text_错误题目计数.gameObject.SetActive(true);
            text_正确题目计数.text = $"正确：{correctCount}道";
            text_错误题目计数.text = $"错误：{wrongCount}道";
        }
        if (hasScore)
        {
            img_总分显示框.gameObject.SetActive(true);
            if (totalScore >= 60)
            {
                img_总分显示框.sprite = sprite_及格;
            }
            else
            {
                img_总分显示框.sprite = sprite_不及格;
            }
            text_总分.text = totalScore.ToString();
            lastScore = totalScore;
        }
    }

    public void OnClick_提交()
    {
        Correct();
        btn_提交按钮.gameObject.SetActive(false);
        btn_返回首页按钮.gameObject.SetActive(true);
        btn_重新考核按钮.gameObject.SetActive(true);
    }
    void OnClick_返回首页()
    {
        btn_提交按钮.gameObject.SetActive(true);
        btn_返回首页按钮.gameObject.SetActive(false);
        btn_重新考核按钮.gameObject.SetActive(false);
        SetActive(false);
    }
    public void OnClick_重新考核()
    {
        List<ExaminationInfo> list = new List<ExaminationInfo>(curInfoList);
        SetExamination(list, hasScore, isRandom_考题, isRandom_选项, examCount);
    }
}



