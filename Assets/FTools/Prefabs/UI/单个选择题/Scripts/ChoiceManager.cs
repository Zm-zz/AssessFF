using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public Sprite sprite_默认;
    public Sprite sprite_正确;
    public Sprite sprite_错误;
    public Sprite sprite_漏选;

    [Space]
    public GameObject panel_考题面板;

    public Text text_考题描述;
    public Transform trans_选项组;
    public GameObject prefab_选项;
    public Button btn_确定按钮;

    ChoiceInfo curInfo;
    bool hasCorrect;
    bool needCorrect;
    Action<bool> correctAction;
    Action closeAction;

    private void Awake()
    {
        btn_确定按钮.onClick.AddListener(OnClick_确定按钮);
    }

    public void Init()
    {
        curInfo = null;
        hasCorrect = false;
        correctAction = null;
        closeAction = null;
        SetActive(false);
    }

    public void SetActive(bool b)
    {
        panel_考题面板.SetActive(b);
    }

    public void SetChoice(ChoiceInfo choiceInfo, Action<bool> correctAction, Action closeAction, bool needCorrect = true)
    {
        SetActive(true);
        hasCorrect = false;
        this.needCorrect = needCorrect;
        this.correctAction = correctAction;
        this.closeAction = closeAction;

        for (int i = trans_选项组.childCount - 1; i >= 0; i--)
        {
            ObjectPoolsManager.Instance.Despawn(trans_选项组.GetChild(i).gameObject);
        }

        curInfo = choiceInfo;
        text_考题描述.text = choiceInfo.topic;
        foreach (var option in curInfo.options)
        {
            GameObject item = ObjectPoolsManager.Instance.Spawn(prefab_选项, trans_选项组);
            item.GetComponentInChildren<Text>().text = option;
            item.GetComponent<Toggle>().isOn = false;
            item.GetComponent<Toggle>().interactable = true;
            item.GetComponent<Image>().sprite = sprite_默认;

            if (choiceInfo.isPicture)
            {
                item.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("PictureTopic/" + option);
            }

            if (choiceInfo.isRadio)
            {
                item.GetComponent<Toggle>().group = trans_选项组.GetComponent<ToggleGroup>();
            }
            else
            {
                item.GetComponent<Toggle>().group = null;
            }
        }
    }

    void OnClick_确定按钮()
    {
        if (GameManager.Instance.mode)
        {
            Correct();
            SetActive(false);
            closeAction?.Invoke();
        }
        else
        {
            //if (hasCorrect)
            //{
            //    SetActive(false);
            //    closeAction?.Invoke();
            //    hasCorrect = false;
            //}
            //else
            //{
            //    Correct();
            //    hasCorrect = true;


            //    if (!needCorrect) OnClick_确定按钮();
            //}

            bool correct = Correct();

            if (correct)
            {
                SetActive(false);
                closeAction?.Invoke();
            }
            else
            {
                Robot.Instance.ShowTipByBut("选择有误，是否跳过", OnSkip, OnCancel);
            }
        }
    }

    private void OnSkip()
    {
        SetActive(false);
        closeAction?.Invoke();
        hasCorrect = false;
    }

    private void OnCancel()
    {
        for (int i = 0; i < trans_选项组.childCount; i++)
        {
            Toggle tog = trans_选项组.GetChild(i).GetComponent<Toggle>();
            tog.interactable = true;
        }
    }

    public bool Correct()
    {
        bool isCorrect = true;
        for (int i = 0; i < trans_选项组.childCount; i++)
        {
            Toggle tog = trans_选项组.GetChild(i).GetComponent<Toggle>();
            Image img = trans_选项组.GetChild(i).GetComponent<Image>();
            if (tog.isOn)
            {
                if (curInfo.answers[i])
                {
                    img.sprite = sprite_正确;
                }
                else
                {
                    img.sprite = sprite_错误;
                    isCorrect = false;
                }
            }
            else
            {
                if (curInfo.answers[i])
                {
                    img.sprite = sprite_漏选;
                    isCorrect = false;
                }
            }
            tog.interactable = false;
        }
        correctAction?.Invoke(isCorrect);

        return isCorrect;
    }

    /// <summary>
    /// 获取所选择的
    /// </summary>
    /// <returns></returns>
    public List<int> GetSelectedByIndex()
    {
        List<int> selected = new List<int>();
        for (int i = 0; i < trans_选项组.childCount; i++)
        {
            Toggle tog = trans_选项组.GetChild(i).GetComponent<Toggle>();
            if (tog.isOn)
            {
                selected.Add(i);
            }
        }

        return selected;
    }

    public List<string> GetSelectedByOption()
    {
        List<string> selected = new List<string>();
        for (int i = 0; i < trans_选项组.childCount; i++)
        {
            Toggle tog = trans_选项组.GetChild(i).GetComponent<Toggle>();
            if (tog.isOn)
            {
                selected.Add(curInfo.options[i]);
            }
        }

        return selected;
    }
}

/// <summary>
/// 选择题数据结构
/// </summary>
public class ChoiceInfo
{
    public string topic;
    public string[] options;
    public bool[] answers;
    public bool isRadio;   //是否单选
    public bool isPicture; // 是否图片考题

    public ChoiceInfo(string topic, string[] options, bool[] answers, bool isRadio = false, bool isPicture = false)
    {
        this.topic = topic;
        this.options = options;
        this.answers = answers;
        this.isRadio = isRadio;
        this.isPicture = isPicture;
    }
    public ChoiceInfo(string topic, string[] options, int answers, bool isRadio = false)
    {
        this.topic = topic;
        this.options = options;
        this.answers = new bool[options.Length];
        while (answers > 0)
        {
            this.answers[(answers % 10) - 1] = true;
            answers /= 10;
        }
        this.isRadio = isRadio;
    }
}

