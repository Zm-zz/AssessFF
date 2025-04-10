using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceManager : MonoBehaviour
{
    public Sprite sprite_Ĭ��;
    public Sprite sprite_��ȷ;
    public Sprite sprite_����;
    public Sprite sprite_©ѡ;

    [Space]
    public GameObject panel_�������;

    public Text text_��������;
    public Transform trans_ѡ����;
    public GameObject prefab_ѡ��;
    public Button btn_ȷ����ť;

    ChoiceInfo curInfo;
    bool hasCorrect;
    bool needCorrect;
    Action<bool> correctAction;
    Action closeAction;

    private void Awake()
    {
        btn_ȷ����ť.onClick.AddListener(OnClick_ȷ����ť);
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
        panel_�������.SetActive(b);
    }

    public void SetChoice(ChoiceInfo choiceInfo, Action<bool> correctAction, Action closeAction, bool needCorrect = true)
    {
        SetActive(true);
        hasCorrect = false;
        this.needCorrect = needCorrect;
        this.correctAction = correctAction;
        this.closeAction = closeAction;

        for (int i = trans_ѡ����.childCount - 1; i >= 0; i--)
        {
            ObjectPoolsManager.Instance.Despawn(trans_ѡ����.GetChild(i).gameObject);
        }

        curInfo = choiceInfo;
        text_��������.text = choiceInfo.topic;
        foreach (var option in curInfo.options)
        {
            GameObject item = ObjectPoolsManager.Instance.Spawn(prefab_ѡ��, trans_ѡ����);
            item.GetComponentInChildren<Text>().text = option;
            item.GetComponent<Toggle>().isOn = false;
            item.GetComponent<Toggle>().interactable = true;
            item.GetComponent<Image>().sprite = sprite_Ĭ��;

            if (choiceInfo.isPicture)
            {
                item.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("PictureTopic/" + option);
            }

            if (choiceInfo.isRadio)
            {
                item.GetComponent<Toggle>().group = trans_ѡ����.GetComponent<ToggleGroup>();
            }
            else
            {
                item.GetComponent<Toggle>().group = null;
            }
        }
    }

    void OnClick_ȷ����ť()
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


            //    if (!needCorrect) OnClick_ȷ����ť();
            //}

            bool correct = Correct();

            if (correct)
            {
                SetActive(false);
                closeAction?.Invoke();
            }
            else
            {
                Robot.Instance.ShowTipByBut("ѡ�������Ƿ�����", OnSkip, OnCancel);
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
        for (int i = 0; i < trans_ѡ����.childCount; i++)
        {
            Toggle tog = trans_ѡ����.GetChild(i).GetComponent<Toggle>();
            tog.interactable = true;
        }
    }

    public bool Correct()
    {
        bool isCorrect = true;
        for (int i = 0; i < trans_ѡ����.childCount; i++)
        {
            Toggle tog = trans_ѡ����.GetChild(i).GetComponent<Toggle>();
            Image img = trans_ѡ����.GetChild(i).GetComponent<Image>();
            if (tog.isOn)
            {
                if (curInfo.answers[i])
                {
                    img.sprite = sprite_��ȷ;
                }
                else
                {
                    img.sprite = sprite_����;
                    isCorrect = false;
                }
            }
            else
            {
                if (curInfo.answers[i])
                {
                    img.sprite = sprite_©ѡ;
                    isCorrect = false;
                }
            }
            tog.interactable = false;
        }
        correctAction?.Invoke(isCorrect);

        return isCorrect;
    }

    /// <summary>
    /// ��ȡ��ѡ���
    /// </summary>
    /// <returns></returns>
    public List<int> GetSelectedByIndex()
    {
        List<int> selected = new List<int>();
        for (int i = 0; i < trans_ѡ����.childCount; i++)
        {
            Toggle tog = trans_ѡ����.GetChild(i).GetComponent<Toggle>();
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
        for (int i = 0; i < trans_ѡ����.childCount; i++)
        {
            Toggle tog = trans_ѡ����.GetChild(i).GetComponent<Toggle>();
            if (tog.isOn)
            {
                selected.Add(curInfo.options[i]);
            }
        }

        return selected;
    }
}

/// <summary>
/// ѡ�������ݽṹ
/// </summary>
public class ChoiceInfo
{
    public string topic;
    public string[] options;
    public bool[] answers;
    public bool isRadio;   //�Ƿ�ѡ
    public bool isPicture; // �Ƿ�ͼƬ����

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

