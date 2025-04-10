using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreController : SingletonPatternMonoBase<ScoreController>
{
    public List<ScoreLibrary> list_ScoreLib;

    public ScoreLibrary scoreLib;

    [Space]
    public bool is_�Ƿ���Ҫ�ϴ��ɼ�;
    public GameObject scorePanel;
    public GameObject scorePanel����;

    [Header("��")]
    public Transform trans_�������ݸ���;
    public Transform trans_�׶�ѡ����;

    [Header("����")]
    public Transform trans_�������ݸ�������;
    public Transform trans_�׶�ѡ��������;

    [Space]
    public GameObject pre_������;
    public GameObject pre_����С��;
    public GameObject pre_�׶ε���;
    public GameObject pre_����������;
    Transform trans_����С���;
    public Text txt_����;
    public Text txt_��������;
    public Button but_Left;
    public Button but_Right;

    public Text txt_����1;
    public Text txt_����2;
    public Text txt_ѧУ1;
    public Text txt_ѧУ2;
    public Text txt_ʱ��1;
    public Text txt_ʱ��2;
    public Text txt_ʱ��1;
    public Text txt_ʱ��2;



    public List<GameObject> list_��ǰ����������;
    public List<Toggle> list_�׶�ѡ��ť;

    [Header("Sprite")]
    public Sprite spr_�׶ε���Nor;
    public Sprite spr_�׶ε���Sel;
    public Sprite spr_�׶ε���Wro;
    public Sprite spr_�÷ֽ���Nor;
    public Sprite spr_�÷ֽ���Wro;
    public Sprite spr_ȫ���ܷ�nor;
    public Sprite spr_ȫ���ܷ�wro;

    [Header("Color")]
    public Color cor_Normal;
    public Color cor_Wrong;

    [Header("����")]
    public Image scoreImg;
    public Sprite goodSprite;
    public Sprite badSprite;
    public Text scoreText;

    private void Start()
    {
        foreach (var item in list_ScoreLib)
        {
            item.ResetScore();
        }
    }

    public void InitScoreLib()
    {
        scoreLib.ResetScore();
    }

    public void SetScoreLib(int index)
    {
        scoreLib = list_ScoreLib[index];
        InitScoreLib();
    }

    public void SetScore(string stageName, string stepName, string pointName, int getIndex, int scoreLibIndex = 0)
    {
        list_ScoreLib[scoreLibIndex][stageName][stepName][pointName].scoreActSet = getIndex;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            SubmitCurrentScore();
        }
    }

    /// <summary>
    /// �ύ��ǰ��ѧ�ɼ�
    /// </summary>
    public void SubmitStep1Score()
    {
        //  GameManager.Instance.kqdx.list_ExamCorrect
        ResultVo resultVo = new ResultVo();
        resultVo.totalScore = 100;
        resultVo.moduleLabel = "DFT";
        List<StepDetail> details = new List<StepDetail>();
        resultVo.details = details;

        //foreach (var correct in GameManager.Instance.kqdx.list_ExamCorrect)
        //{
        //    StepDetail detail = new StepDetail();
        //    detail.done = true;
        //    List<PointDetail> pointDetails = new List<PointDetail>();
        //    PointDetail pointDetail = new PointDetail();
        //    pointDetail.action = correct ? "����ش���ȷ" : "����ش����";
        //    pointDetail.total = 10;
        //    pointDetail.scoreGet = correct ? 10 : 0;
        //    detail.pointDetails = pointDetails;
        //    pointDetails.Add(pointDetail);
        //    details.Add(detail);
        //}

        Backend.Instance.Submit(resultVo);

        Debug.Log(resultVo.details.Count);
        foreach (var stepDetail in resultVo.details)
        {
            foreach (var item in stepDetail.pointDetails)
            {
                Debug.Log("��Ϊ��" + item.action + "�׶��ܷ�: " + item.total + "�÷�: " + item.scoreGet);
            }
        }
    }

    public void SubmitCurrentScore()
    {
        ResultVo resultVo = new ResultVo();
        resultVo.totalScore = scoreLib.GetScoreTotal();
        resultVo.moduleLabel = scoreLib.scoreID;
        List<StepDetail> details = new List<StepDetail>();
        resultVo.details = details;
        foreach (var stage in scoreLib.resultVo)
        {
            StepDetail detail = new StepDetail();
            detail.done = true;
            foreach (var step in stage.stepInfos)
            {
                List<PointDetail> pointDetails = new List<PointDetail>();
                foreach (var point in step.pointDatas)
                {
                    PointDetail pointDetail = new PointDetail();
                    pointDetail.action = point.GetInfo();
                    pointDetail.total = point.GetTotalScore();
                    pointDetail.scoreGet = point.GetScore();

                    pointDetails.Add(pointDetail);
                }
                detail.pointDetails = pointDetails;
            }
            details.Add(detail);
        }

        Backend.Instance.Submit(resultVo);

        Debug.Log(resultVo.details.Count);
        foreach (var stepDetail in resultVo.details)
        {
            foreach (var item in stepDetail.pointDetails)
            {
                Debug.Log("��Ϊ��" + item.action + "�׶��ܷ�: " + item.total + "�÷�: " + item.scoreGet);
            }
        }
    }

    public void SubmitScore()
    {
        ClearScore();
        SetScore();

        if (is_�Ƿ���Ҫ�ϴ��ɼ�)
        {
            //////////////Backend.Instance.BeginTrain(isTrain);!!!!!!!!!!!!!
            //////////////��Ҫָ������ģʽ��ѵ��ģʽ
            //resultVo.totalScore = scoreLib.GetScoreTotal();
            //resultVo.moduleLabel = scoreLib.scoreID;
            //Backend.Instance.Submit(resultVo);
        }
    }

    /// <summary>
    /// �ڶ��׶�����
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScore(UnityAction left = null, UnityAction right = null, string leftContent = "���ش���", string rightContent = "����ʵսģ��")
    {
        //txt_����1.text = GameManager.Instance.UserName;
        //txt_ѧУ1.text = GameManager.Instance.UserSchool;
        //txt_ʱ��1.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_ʱ��1.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel.SetActive(true);
        SetScore();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }


    /// <summary>
    /// �����׶�����
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScoreFor3(UnityAction left = null, UnityAction right = null, string leftContent = "���ش���", string rightContent = "����ʵսģ��")
    {
        //txt_����2.text = GameManager.Instance.UserName;
        //txt_ѧУ2.text = GameManager.Instance.UserSchool;
        //txt_ʱ��2.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_ʱ��2.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel����.SetActive(true);
        SetScore����();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }

    /// <summary>
    /// չʾ�������ڷ���
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScoreFor34Together(UnityAction left = null, UnityAction right = null, string leftContent = "���ش���", string rightContent = "����ʵսģ��")
    {
        //txt_����2.text = GameManager.Instance.UserName;
        //txt_ѧУ2.text = GameManager.Instance.UserSchool;
        //txt_ʱ��2.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_ʱ��2.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel����.SetActive(true);
        SetScore����Together();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }

    public void CloseScore()
    {
        scorePanel.SetActive(false);
        scorePanel����.SetActive(false);

        but_Left.gameObject.SetActive(false);
        but_Right.gameObject.SetActive(false);
    }

    void SetScore()
    {
        CreateScoreTable(scoreLib, trans_�׶�ѡ����, trans_�������ݸ���);
    }

    void SetScore����()
    {
        Image img_�ܷ� = transform.Find("���ֱ�����/���ݲ���/�ܷ�").GetComponent<Image>();
        img_�ܷ�.sprite = list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 ? spr_ȫ���ܷ�nor : spr_ȫ���ܷ�wro;
        transform.Find("���ֱ�����/���ݲ���/�ܷ�/����").GetComponent<Text>().text = (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet()).ToString();

        if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 60)
        {
            txt_��������.text = "Ŭ�����´μ��ͣ�";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 && list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 90)
        {
            txt_��������.text = "�ܰ�������һ��¥��";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 90)
        {
            txt_��������.text = "���㣬����Ŭ����";
        }

        Dropdown dropdown = transform.Find("���ֱ�����/���ݲ���/������").GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>() { scoreLib.scoreName, list_ScoreLib[0].scoreName });
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((int value) =>
        {
            if (value == 1)
            {
                CreateScoreTable(list_ScoreLib[0], trans_�׶�ѡ��������, trans_�������ݸ�������);
            }
            else
            {
                CreateScoreTable(scoreLib, trans_�׶�ѡ��������, trans_�������ݸ�������);
            }
        });

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        CreateScoreTable(scoreLib, trans_�׶�ѡ��������, trans_�������ݸ�������);
    }

    void SetScore����Together()
    {
        Image img_�ܷ� = transform.Find("���ֱ�����/���ݲ���/�ܷ�").GetComponent<Image>();
        img_�ܷ�.sprite = list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 ? spr_ȫ���ܷ�nor : spr_ȫ���ܷ�wro;
        transform.Find("���ֱ�����/���ݲ���/�ܷ�/����").GetComponent<Text>().text = (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet()).ToString();

        if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 60)
        {
            txt_��������.text = "Ŭ�����´μ��ͣ�";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 && list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 90)
        {
            txt_��������.text = "�ܰ�������һ��¥��";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 90)
        {
            txt_��������.text = "���㣬����Ŭ����";
        }

        Dropdown dropdown = transform.Find("���ֱ�����/���ݲ���/������").GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>() { scoreLib.scoreName, list_ScoreLib[0].scoreName, list_ScoreLib[1].scoreName });
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((int value) =>
        {
            if (value == 2)
            {
                CreateScoreTable(list_ScoreLib[1], trans_�׶�ѡ��������, trans_�������ݸ�������);
            }
            else if (value == 1)
            {
                CreateScoreTable(list_ScoreLib[0], trans_�׶�ѡ��������, trans_�������ݸ�������);
            }
            else
            {
                CreateScoreTable(scoreLib, trans_�׶�ѡ��������, trans_�������ݸ�������);
            }
        });

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        CreateScoreTable(scoreLib, trans_�׶�ѡ��������, trans_�������ݸ�������);
    }

    void OnClick_���()
    {
        scorePanel.SetActive(false);
    }

    /// <summary>
    /// ���ð�ť
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    private void SetButton(UnityAction left, UnityAction right, string leftContent = "���ش���", string rightContent = "����ʵսģ��")
    {
        but_Left.onClick.RemoveAllListeners();
        but_Right.onClick.RemoveAllListeners();

        but_Left.onClick.AddListener(left);
        but_Right.onClick.AddListener(right);
        but_Left.GetComponentInChildren<Text>().text = leftContent;
        but_Right.GetComponentInChildren<Text>().text = rightContent;
    }

    /// <summary>
    /// �����÷ֱ�
    /// </summary>
    private void CreateScoreTable(ScoreLibrary scoreLib, Transform trans_�׶�ѡ����, Transform trans_�������ݸ���)
    {
        CreateScoreTableContentExceptScorePoint(scoreLib, trans_�׶�ѡ����, trans_�������ݸ���);

        // ����ݽ׶�ѡ�񿪹�ѡ��������������ɵ÷����������ע��
        // CreateAllItem(scoreLib);
    }

    /// <summary>
    /// �����÷ֱ����ݣ����˸����÷ֵ㣩
    /// </summary>
    private void CreateScoreTableContentExceptScorePoint(ScoreLibrary scoreLib, Transform trans_�׶θ���, Transform trans_�������ݸ���)
    {
        #region �����˹���
        if (scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f)
        {
            txt_����.text = "Ŭ�����´μ��ͣ�";
        }
        else if (scoreLib.GetScoreGet() >= scoreLib.GetScoreTotal() * 0.6f && scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.9f)
        {
            txt_����.text = "�ܰ�������һ��¥��";
        }
        else if (scoreLib.GetScoreGet() >= scoreLib.GetScoreTotal() * 0.9f)
        {
            txt_����.text = "���㣬����Ŭ����";
        }
        #endregion

        foreach (Toggle item in list_�׶�ѡ��ť)
        {
            DestroyImmediate(item.gameObject);
        }
        list_�׶�ѡ��ť.Clear();

        for (int i = 0; i < trans_�׶θ���.childCount; i++)
        {
            DestroyImmediate(trans_�׶θ���.GetChild(i).gameObject);
        }

        ToggleGroup togGroup = trans_�׶θ���.GetComponent<ToggleGroup>();

        GameObject totalScore = ObjectPoolsManager.Instance.Spawn(pre_�׶ε���, trans_�׶θ���);

        Toggle totalTog = totalScore.GetComponent<Toggle>();
        totalTog.group = togGroup;
        totalTog.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                totalTog.GetComponent<Image>().sprite = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? spr_�׶ε���Wro : spr_�׶ε���Sel;
                CreateAllItem(scoreLib, trans_�������ݸ���);
            }
            else
            {
                totalTog.GetComponent<Image>().sprite = spr_�׶ε���Nor;
            }
        });
        totalTog.isOn = true;
        list_�׶�ѡ��ť.Add(totalTog);

        totalScore.transform.Find("����").GetComponent<Text>().text = "�ܷ�";
        totalScore.transform.Find("����").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;
        totalScore.transform.Find("��ֵ").GetComponent<Text>().text = scoreLib.GetScoreGet().ToString();
        totalScore.transform.Find("��ֵ").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;
        totalScore.transform.Find("���ֽ���/����").GetComponent<Image>().fillAmount = scoreLib.GetScoreGet() / scoreLib.GetScoreTotal();
        totalScore.transform.Find("���ֽ���/����").GetComponent<Image>().sprite = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? spr_�÷ֽ���Wro : spr_�÷ֽ���Nor;
        totalScore.transform.Find("���ֽ���/���Ȱٷֱ�").GetComponent<Text>().text = (int)((scoreLib.GetScoreGet() / scoreLib.GetScoreTotal()) * 100) + "%";
        totalScore.transform.Find("���ֽ���/���Ȱٷֱ�").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;

        ObjectPoolsManager.Instance.Spawn(pre_����������, trans_�׶θ���);

        foreach (var stage in scoreLib.resultVo)
        {
            GameObject stepSelect = ObjectPoolsManager.Instance.Spawn(pre_�׶ε���, trans_�׶θ���);

            Toggle stepTog = stepSelect.GetComponent<Toggle>();
            stepTog.group = togGroup;
            stepTog.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    stepTog.GetComponent<Image>().sprite = spr_�׶ε���Sel;
                    CreateItem(scoreLib, stage.stageName, trans_�������ݸ���);
                }
                else
                {
                    stepTog.GetComponent<Image>().sprite = spr_�׶ε���Nor;
                }
            });
            list_�׶�ѡ��ť.Add(stepTog);

            stepSelect.transform.Find("����").GetComponent<Text>().text = stage.stageName;
            stepSelect.transform.Find("��ֵ").GetComponent<Text>().text = stage.GetScoreGet().ToString();
            stepSelect.transform.Find("���ֽ���/����").GetComponent<Image>().fillAmount = stage.GetScoreGet() / stage.GetScoreTotal();
            stepSelect.transform.Find("���ֽ���/���Ȱٷֱ�").GetComponent<Text>().text = (int)((stage.GetScoreGet() / stage.GetScoreTotal()) * 100) + "%";
        }
    }

    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="scoreLib"></param>
    private void CreateAllItem(ScoreLibrary scoreLib, Transform trans_�������ݸ���)
    {
        // ����б�
        foreach (var item in list_��ǰ����������)
        {
            DestroyImmediate(item);
        }
        list_��ǰ����������.Clear();

        foreach (var stage in scoreLib.resultVo)
        {
            StageAdd(stage, trans_�������ݸ���);
        }
    }

    /// <summary>
    /// ��������������
    /// </summary>
    /// <param name="scoreLib"></param>
    /// <param name="stageName"></param>
    private void CreateItem(ScoreLibrary scoreLib, string stageName, Transform trans_�������ݸ���)
    {
        // ����б�
        foreach (var item in list_��ǰ����������)
        {
            DestroyImmediate(item);
        }
        list_��ǰ����������.Clear();

        foreach (var stage in scoreLib.resultVo)
        {
            if (stage.stageName == stageName)
            {
                StageAdd(stage, trans_�������ݸ���);
            }
        }
    }

    /// <summary>
    /// ��ӵ÷���
    /// </summary>
    /// <param name="stage"></param>
    private void StageAdd(ScoreStageInfo stage, Transform trans_�������ݸ���)
    {
        GameObject scoreItem = ObjectPoolsManager.Instance.Spawn(pre_������, trans_�������ݸ���);
        scoreItem.transform.Find("������/����").GetComponent<Text>().text = stage.stageName;
        trans_����С��� = scoreItem.transform.Find("������/С����").transform;
        foreach (var item in stage.stepInfos)
        {
            StepAdd(trans_����С���, item);
        }

        list_��ǰ����������.Add(scoreItem);

        LayoutRebuilder.ForceRebuildLayoutImmediate(scoreItem.GetComponent<RectTransform>());
    }

    /// <summary>
    /// ��ӵ÷ֵ���
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="step"></param>
    private void StepAdd(Transform parent, ScoreStepInfo step)
    {
        foreach (var point in step.pointDatas)
        {
            GameObject pointItem = ObjectPoolsManager.Instance.Spawn(pre_����С��, parent);
            pointItem.transform.Find("��������").GetComponent<Text>().text = point.GetInfo();
            pointItem.transform.Find("�÷�").GetComponent<Text>().text = point.GetScore().ToString() + "��";
            pointItem.transform.Find("��ֵ").GetComponent<Text>().text = point.GetTotalScore().ToString() + "��";

            //if (point.GetScore() != point.GetTotalScore())
            //{
            //    pointItem.transform.Find("��������").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            //    pointItem.transform.Find("�÷�").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            //}
            //else
            //{
            //    pointItem.transform.Find("��������").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
            //    pointItem.transform.Find("�÷�").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
            //}

            LayoutRebuilder.ForceRebuildLayoutImmediate(pointItem.GetComponent<RectTransform>());
        }
    }

    private PointDetail SubmitPoint(ScorePointData pointData)
    {
        PointDetail pointDetail = new PointDetail();
        pointDetail.action = pointData.GetInfo();
        pointDetail.total = pointData.GetTotalScore();
        pointDetail.scoreGet = pointData.GetScore();
        return pointDetail;
    }

    private void ClearScore()
    {
        Transform[] objs = trans_�������ݸ���.GetComponentsInChildren<Transform>();
        for (int i = 1; i < objs.Length; i++)
        {
            ObjectPoolsManager.Instance.Despawn(objs[i].gameObject);
        }
        //resultVo.details.Clear();
        //resultVo.details = new List<StepDetail>();
    }

}
