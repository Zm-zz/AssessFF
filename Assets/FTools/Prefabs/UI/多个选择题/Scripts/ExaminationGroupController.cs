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
    
    //�����ȡ
    bool hasScore;
    bool isRandom_����;
    bool isRandom_ѡ��;
    int examCount;
    List<ExaminationInfo> curInfoList;
    [Space]
    [SerializeField] private Sprite sprite_��Ŀ��ȷ;
    [SerializeField] private Sprite sprite_��Ŀ����;
    [SerializeField] private Sprite sprite_��ѡĬ��;
    [SerializeField] private Sprite sprite_��ѡ����;
    [SerializeField] private Sprite sprite_��ѡѡ��;
    [SerializeField] private Sprite sprite_��ѡ��ȷ;
    [SerializeField] private Sprite sprite_��ѡ����;
    [SerializeField] private Sprite sprite_��ѡ©ѡ;
    [SerializeField] private Sprite sprite_��ѡĬ��;
    [SerializeField] private Sprite sprite_��ѡ����;
    [SerializeField] private Sprite sprite_��ѡѡ��;
    [SerializeField] private Sprite sprite_��ѡ��ȷ;
    [SerializeField] private Sprite sprite_��ѡ����;
    [SerializeField] private Sprite sprite_��ѡ©ѡ;
    [SerializeField] private Color color_��ȷ;
    [SerializeField] private Color color_����;
    [SerializeField] private Color color_©ѡ;
    [SerializeField] private Color color_Ĭ��;
    [Space]
    [SerializeField] private GameObject panel_���������;
    [SerializeField] private Text text_����Ŀ����;
    [SerializeField] private Text text_��ȷ��Ŀ����;
    [SerializeField] private Text text_������Ŀ����;

    [SerializeField] private GameObject prefab_������;
    [SerializeField] private GameObject prefab_����ѡ��;
    [SerializeField] private Transform trans_���⸸;

    [SerializeField] private Image img_�ܷ���ʾ��;
    [SerializeField] private Sprite sprite_����;
    [SerializeField] private Sprite sprite_������;
    [SerializeField] private Text text_�ܷ�;

    [SerializeField] private Button btn_�ύ��ť;
    [SerializeField] private Button btn_���¿��˰�ť;
    [SerializeField] private Button btn_������ҳ��ť;

    List<ExaminationInfo> examinationInfoList;
    List<GameObject> examinationObjList;

    [HideInInspector] public int lastScore;

    private void Awake()
    {
        examinationInfoList = new List<ExaminationInfo>();
        examinationObjList = new List<GameObject>();
        btn_�ύ��ť.onClick.AddListener(OnClick_�ύ);
        btn_������ҳ��ť.onClick.AddListener(OnClick_������ҳ);
        btn_���¿��˰�ť.onClick.AddListener(OnClick_���¿���);
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
    public void SetExamination(List<ExaminationInfo> list, bool hasScore = false, bool isRandom_���� = false, bool isRandom_ѡ�� = false, int examCount = -1)
    {
        this.hasScore = hasScore;
        this.isRandom_���� = isRandom_����;
        this.isRandom_ѡ�� = isRandom_ѡ��;
        this.examCount = examCount;
        curInfoList = new List<ExaminationInfo>(list);
        if (examCount > list.Count)
        {
            Debug.LogError("�����ȡ����");
            examCount = -1;
        }
        if (examCount == -1) examCount = list.Count;
        if (isRandom_ѡ��)
        {
            List<ExaminationInfo> eInfo = new List<ExaminationInfo>();
            for (int index = 0; index < list.Count; index++)
            {
                ExaminationInfo eIndex = list[index];
                string[] options = new string[eIndex.options.Length];
                bool[] answers = new bool[eIndex.answers.Length];
                //����ѡ��ʹ�
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
        btn_�ύ��ť.gameObject.SetActive(true);
        btn_������ҳ��ť.gameObject.SetActive(false);
        btn_���¿��˰�ť.gameObject.SetActive(false);
        foreach (var obj in examinationObjList)
        {
            Destroy(obj);
        }
        examinationObjList.Clear();
        examinationInfoList.Clear();
        examinationInfoList = list;
        examinationInfoList = isRandom_���� ? FMethod.RandomList(list).GetRange(0, examCount) : list.GetRange(0, examCount);

        img_�ܷ���ʾ��.gameObject.SetActive(false);
        foreach (var examination in examinationInfoList)
        {
            GameObject exam = Instantiate(prefab_������, trans_���⸸);
            exam.transform.Find("������/����").GetComponent<Text>().text = examination.topic;
            Transform trans_ѡ� = exam.transform.Find("������/ѡ����");
            for (int i = 0; i < trans_ѡ�.childCount; i++)
            {
                Destroy(trans_ѡ�.GetChild(i).gameObject);
            }
            for (int i = 0; i < examination.options.Length; i++)
            {
                GameObject item = Instantiate(prefab_����ѡ��, trans_ѡ�);
                item.transform.GetComponentInChildren<Text>().text = examination.options[i];
                item.GetComponentInChildren<Text>().color = Color.black;
                item.GetComponent<Toggle>().interactable = true;
                if (examination.isRadio)
                {
                    item.GetComponent<Toggle>().group = trans_ѡ�.GetComponent<ToggleGroup>();
                    item.GetComponentInChildren<Image>().sprite = sprite_��ѡĬ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_off = sprite_��ѡĬ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_on = sprite_��ѡѡ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_highlight_off = sprite_��ѡ����;
                }
                else
                {
                    item.GetComponent<Toggle>().group = null;
                    item.GetComponentInChildren<Image>().sprite = sprite_��ѡĬ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_off = sprite_��ѡĬ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_on = sprite_��ѡѡ��;
                    item.GetComponentInChildren<ToggleValueChange>().sprite_highlight_off = sprite_��ѡ����;
                }
            }
            exam.transform.Find("������ʾ").gameObject.SetActive(false);
            examinationObjList.Add(exam);
        }
        text_����Ŀ����.gameObject.SetActive(true);
        text_��ȷ��Ŀ����.gameObject.SetActive(false);
        text_������Ŀ����.gameObject.SetActive(false);
        text_����Ŀ����.text = $"��{examinationInfoList.Count}��";
    }
    public void SetExamination(string examGroupName, bool hasScore = false, bool isRandom_���� = false, bool isRandom_ѡ�� = false, int examCount = -1)
    {
        SetExamination(GetExaminations(examGroupName), hasScore, isRandom_����, isRandom_ѡ��, examCount);
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
            ExaminationInfo info = examinationInfoList[i];
            GameObject obj = examinationObjList[i];
            for (int j = 0; j < info.options.Length; j++)
            {

                GameObject item = obj.transform.Find("������/ѡ����").GetChild(j).gameObject;
                if (info.answers[j])
                {
                    if (item.GetComponent<Toggle>().isOn)
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ��ȷ;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ��ȷ;
                        }
                        item.GetComponentInChildren<Text>().color = color_��ȷ;
                    }
                    else
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ©ѡ;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ©ѡ;
                        }
                        item.GetComponentInChildren<Text>().color = color_©ѡ;
                        isCorrect = false;
                    }
                }
                else
                {
                    if (item.GetComponent<Toggle>().isOn)
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ����;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡ����;
                        }
                        item.GetComponentInChildren<Image>().sprite = sprite_��ѡ����;
                        item.GetComponentInChildren<Text>().color = color_����;
                        isCorrect = false;
                    }
                    else
                    {
                        if (info.isRadio)
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡĬ��;
                        }
                        else
                        {
                            item.GetComponentInChildren<Image>().sprite = sprite_��ѡĬ��;
                        }
                        item.GetComponentInChildren<Text>().color = color_Ĭ��;
                    }
                }
                item.GetComponent<Toggle>().interactable = false;
            }
            obj.transform.Find("������ʾ").gameObject.SetActive(true);
            if (isCorrect)
            {
                obj.transform.Find("������ʾ").GetComponent<Image>().sprite = sprite_��Ŀ��ȷ;
                totalScore += info.score;
                correctCount++;
            }
            else
            {
                obj.transform.Find("������ʾ").GetComponent<Image>().sprite = sprite_��Ŀ����;
                wrongCount++;
            }
            text_����Ŀ����.gameObject.SetActive(false);
            text_��ȷ��Ŀ����.gameObject.SetActive(true);
            text_������Ŀ����.gameObject.SetActive(true);
            text_��ȷ��Ŀ����.text = $"��ȷ��{correctCount}��";
            text_������Ŀ����.text = $"����{wrongCount}��";
        }
        if (hasScore)
        {
            img_�ܷ���ʾ��.gameObject.SetActive(true);
            if (totalScore >= 60)
            {
                img_�ܷ���ʾ��.sprite = sprite_����;
            }
            else
            {
                img_�ܷ���ʾ��.sprite = sprite_������;
            }
            text_�ܷ�.text = totalScore.ToString();
            lastScore = totalScore;
        }
    }

    public void OnClick_�ύ()
    {
        Correct();
        btn_�ύ��ť.gameObject.SetActive(false);
        btn_������ҳ��ť.gameObject.SetActive(true);
        btn_���¿��˰�ť.gameObject.SetActive(true);
    }
    void OnClick_������ҳ()
    {
        btn_�ύ��ť.gameObject.SetActive(true);
        btn_������ҳ��ť.gameObject.SetActive(false);
        btn_���¿��˰�ť.gameObject.SetActive(false);
        SetActive(false);
    }
    public void OnClick_���¿���()
    {
        List<ExaminationInfo> list = new List<ExaminationInfo>(curInfoList);
        SetExamination(list, hasScore, isRandom_����, isRandom_ѡ��, examCount);
    }
}



