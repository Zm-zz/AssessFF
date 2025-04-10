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
    public bool is_是否需要上传成绩;
    public GameObject scorePanel;
    public GameObject scorePanel三四;

    [Header("二")]
    public Transform trans_评分内容父类;
    public Transform trans_阶段选择父类;

    [Header("三四")]
    public Transform trans_评分内容父类三四;
    public Transform trans_阶段选择父类三四;

    [Space]
    public GameObject pre_评分项;
    public GameObject pre_评分小项;
    public GameObject pre_阶段单项;
    public GameObject pre_评分里竖线;
    Transform trans_评分小项父类;
    public Text txt_鼓励;
    public Text txt_鼓励三四;
    public Button but_Left;
    public Button but_Right;

    public Text txt_姓名1;
    public Text txt_姓名2;
    public Text txt_学校1;
    public Text txt_学校2;
    public Text txt_时长1;
    public Text txt_时长2;
    public Text txt_时间1;
    public Text txt_时间2;



    public List<GameObject> list_当前评分项内容;
    public List<Toggle> list_阶段选择按钮;

    [Header("Sprite")]
    public Sprite spr_阶段单项Nor;
    public Sprite spr_阶段单项Sel;
    public Sprite spr_阶段单项Wro;
    public Sprite spr_得分进度Nor;
    public Sprite spr_得分进度Wro;
    public Sprite spr_全部总分nor;
    public Sprite spr_全部总分wro;

    [Header("Color")]
    public Color cor_Normal;
    public Color cor_Wrong;

    [Header("分数")]
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
    /// 提交课前导学成绩
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
        //    pointDetail.action = correct ? "考题回答正确" : "考题回答错误";
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
                Debug.Log("行为：" + item.action + "阶段总分: " + item.total + "得分: " + item.scoreGet);
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
                Debug.Log("行为：" + item.action + "阶段总分: " + item.total + "得分: " + item.scoreGet);
            }
        }
    }

    public void SubmitScore()
    {
        ClearScore();
        SetScore();

        if (is_是否需要上传成绩)
        {
            //////////////Backend.Instance.BeginTrain(isTrain);!!!!!!!!!!!!!
            //////////////需要指定考核模式或训练模式
            //resultVo.totalScore = scoreLib.GetScoreTotal();
            //resultVo.moduleLabel = scoreLib.scoreID;
            //Backend.Instance.Submit(resultVo);
        }
    }

    /// <summary>
    /// 第二阶段评分
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScore(UnityAction left = null, UnityAction right = null, string leftContent = "返回大厅", string rightContent = "继续实战模块")
    {
        //txt_姓名1.text = GameManager.Instance.UserName;
        //txt_学校1.text = GameManager.Instance.UserSchool;
        //txt_时长1.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_时间1.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel.SetActive(true);
        SetScore();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }


    /// <summary>
    /// 第三阶段评分
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScoreFor3(UnityAction left = null, UnityAction right = null, string leftContent = "返回大厅", string rightContent = "继续实战模块")
    {
        //txt_姓名2.text = GameManager.Instance.UserName;
        //txt_学校2.text = GameManager.Instance.UserSchool;
        //txt_时长2.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_时间2.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel三四.SetActive(true);
        SetScore三四();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }

    /// <summary>
    /// 展示三个环节分数
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    public void ShowScoreFor34Together(UnityAction left = null, UnityAction right = null, string leftContent = "返回大厅", string rightContent = "继续实战模块")
    {
        //txt_姓名2.text = GameManager.Instance.UserName;
        //txt_学校2.text = GameManager.Instance.UserSchool;
        //txt_时长2.text = GameManager.Instance.ConvertSecondsToTime(GameManager.Instance.ShowTime() - GameManager.Instance.startTime);
        txt_时间2.text = DateTime.Now.ToString("yyyy-MM-dd");

        scorePanel三四.SetActive(true);
        SetScore三四Together();

        but_Left.gameObject.SetActive(true);
        but_Right.gameObject.SetActive(true);
        SetButton(left, right, leftContent, rightContent);

        SubmitCurrentScore();
    }

    public void CloseScore()
    {
        scorePanel.SetActive(false);
        scorePanel三四.SetActive(false);

        but_Left.gameObject.SetActive(false);
        but_Right.gameObject.SetActive(false);
    }

    void SetScore()
    {
        CreateScoreTable(scoreLib, trans_阶段选择父类, trans_评分内容父类);
    }

    void SetScore三四()
    {
        Image img_总分 = transform.Find("评分表三四/内容部分/总分").GetComponent<Image>();
        img_总分.sprite = list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 ? spr_全部总分nor : spr_全部总分wro;
        transform.Find("评分表三四/内容部分/总分/分数").GetComponent<Text>().text = (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet()).ToString();

        if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 60)
        {
            txt_鼓励三四.text = "努力，下次加油！";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 && list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 90)
        {
            txt_鼓励三四.text = "很棒，再上一层楼！";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 90)
        {
            txt_鼓励三四.text = "优秀，继续努力！";
        }

        Dropdown dropdown = transform.Find("评分表三四/内容部分/下拉框").GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>() { scoreLib.scoreName, list_ScoreLib[0].scoreName });
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((int value) =>
        {
            if (value == 1)
            {
                CreateScoreTable(list_ScoreLib[0], trans_阶段选择父类三四, trans_评分内容父类三四);
            }
            else
            {
                CreateScoreTable(scoreLib, trans_阶段选择父类三四, trans_评分内容父类三四);
            }
        });

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        CreateScoreTable(scoreLib, trans_阶段选择父类三四, trans_评分内容父类三四);
    }

    void SetScore三四Together()
    {
        Image img_总分 = transform.Find("评分表三四/内容部分/总分").GetComponent<Image>();
        img_总分.sprite = list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 ? spr_全部总分nor : spr_全部总分wro;
        transform.Find("评分表三四/内容部分/总分/分数").GetComponent<Text>().text = (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet()).ToString();

        if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 60)
        {
            txt_鼓励三四.text = "努力，下次加油！";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 60 && list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() < 90)
        {
            txt_鼓励三四.text = "很棒，再上一层楼！";
        }
        else if (list_ScoreLib[0].GetScoreGet() + scoreLib.GetScoreGet() >= 90)
        {
            txt_鼓励三四.text = "优秀，继续努力！";
        }

        Dropdown dropdown = transform.Find("评分表三四/内容部分/下拉框").GetComponent<Dropdown>();
        dropdown.ClearOptions();
        dropdown.AddOptions(new List<string>() { scoreLib.scoreName, list_ScoreLib[0].scoreName, list_ScoreLib[1].scoreName });
        dropdown.onValueChanged.RemoveAllListeners();
        dropdown.onValueChanged.AddListener((int value) =>
        {
            if (value == 2)
            {
                CreateScoreTable(list_ScoreLib[1], trans_阶段选择父类三四, trans_评分内容父类三四);
            }
            else if (value == 1)
            {
                CreateScoreTable(list_ScoreLib[0], trans_阶段选择父类三四, trans_评分内容父类三四);
            }
            else
            {
                CreateScoreTable(scoreLib, trans_阶段选择父类三四, trans_评分内容父类三四);
            }
        });

        dropdown.value = 0;
        dropdown.RefreshShownValue();
        CreateScoreTable(scoreLib, trans_阶段选择父类三四, trans_评分内容父类三四);
    }

    void OnClick_完成()
    {
        scorePanel.SetActive(false);
    }

    /// <summary>
    /// 设置按钮
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <param name="leftContent"></param>
    /// <param name="rightContent"></param>
    private void SetButton(UnityAction left, UnityAction right, string leftContent = "返回大厅", string rightContent = "继续实战模块")
    {
        but_Left.onClick.RemoveAllListeners();
        but_Right.onClick.RemoveAllListeners();

        but_Left.onClick.AddListener(left);
        but_Right.onClick.AddListener(right);
        but_Left.GetComponentInChildren<Text>().text = leftContent;
        but_Right.GetComponentInChildren<Text>().text = rightContent;
    }

    /// <summary>
    /// 创建得分表
    /// </summary>
    private void CreateScoreTable(ScoreLibrary scoreLib, Transform trans_阶段选择父类, Transform trans_评分内容父类)
    {
        CreateScoreTableContentExceptScorePoint(scoreLib, trans_阶段选择父类, trans_评分内容父类);

        // 会根据阶段选择开关选中与否来调用生成得分项函数，所以注释
        // CreateAllItem(scoreLib);
    }

    /// <summary>
    /// 创建得分表内容（除了各个得分点）
    /// </summary>
    private void CreateScoreTableContentExceptScorePoint(ScoreLibrary scoreLib, Transform trans_阶段父类, Transform trans_评分内容父类)
    {
        #region 机器人鼓励
        if (scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f)
        {
            txt_鼓励.text = "努力，下次加油！";
        }
        else if (scoreLib.GetScoreGet() >= scoreLib.GetScoreTotal() * 0.6f && scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.9f)
        {
            txt_鼓励.text = "很棒，再上一层楼！";
        }
        else if (scoreLib.GetScoreGet() >= scoreLib.GetScoreTotal() * 0.9f)
        {
            txt_鼓励.text = "优秀，继续努力！";
        }
        #endregion

        foreach (Toggle item in list_阶段选择按钮)
        {
            DestroyImmediate(item.gameObject);
        }
        list_阶段选择按钮.Clear();

        for (int i = 0; i < trans_阶段父类.childCount; i++)
        {
            DestroyImmediate(trans_阶段父类.GetChild(i).gameObject);
        }

        ToggleGroup togGroup = trans_阶段父类.GetComponent<ToggleGroup>();

        GameObject totalScore = ObjectPoolsManager.Instance.Spawn(pre_阶段单项, trans_阶段父类);

        Toggle totalTog = totalScore.GetComponent<Toggle>();
        totalTog.group = togGroup;
        totalTog.onValueChanged.AddListener((bool isOn) =>
        {
            if (isOn)
            {
                totalTog.GetComponent<Image>().sprite = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? spr_阶段单项Wro : spr_阶段单项Sel;
                CreateAllItem(scoreLib, trans_评分内容父类);
            }
            else
            {
                totalTog.GetComponent<Image>().sprite = spr_阶段单项Nor;
            }
        });
        totalTog.isOn = true;
        list_阶段选择按钮.Add(totalTog);

        totalScore.transform.Find("标题").GetComponent<Text>().text = "总分";
        totalScore.transform.Find("标题").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;
        totalScore.transform.Find("分值").GetComponent<Text>().text = scoreLib.GetScoreGet().ToString();
        totalScore.transform.Find("分值").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;
        totalScore.transform.Find("评分进度/进度").GetComponent<Image>().fillAmount = scoreLib.GetScoreGet() / scoreLib.GetScoreTotal();
        totalScore.transform.Find("评分进度/进度").GetComponent<Image>().sprite = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? spr_得分进度Wro : spr_得分进度Nor;
        totalScore.transform.Find("评分进度/进度百分比").GetComponent<Text>().text = (int)((scoreLib.GetScoreGet() / scoreLib.GetScoreTotal()) * 100) + "%";
        totalScore.transform.Find("评分进度/进度百分比").GetComponent<Text>().color = scoreLib.GetScoreGet() < scoreLib.GetScoreTotal() * 0.6f ? cor_Wrong : cor_Normal;

        ObjectPoolsManager.Instance.Spawn(pre_评分里竖线, trans_阶段父类);

        foreach (var stage in scoreLib.resultVo)
        {
            GameObject stepSelect = ObjectPoolsManager.Instance.Spawn(pre_阶段单项, trans_阶段父类);

            Toggle stepTog = stepSelect.GetComponent<Toggle>();
            stepTog.group = togGroup;
            stepTog.onValueChanged.AddListener((bool isOn) =>
            {
                if (isOn)
                {
                    stepTog.GetComponent<Image>().sprite = spr_阶段单项Sel;
                    CreateItem(scoreLib, stage.stageName, trans_评分内容父类);
                }
                else
                {
                    stepTog.GetComponent<Image>().sprite = spr_阶段单项Nor;
                }
            });
            list_阶段选择按钮.Add(stepTog);

            stepSelect.transform.Find("标题").GetComponent<Text>().text = stage.stageName;
            stepSelect.transform.Find("分值").GetComponent<Text>().text = stage.GetScoreGet().ToString();
            stepSelect.transform.Find("评分进度/进度").GetComponent<Image>().fillAmount = stage.GetScoreGet() / stage.GetScoreTotal();
            stepSelect.transform.Find("评分进度/进度百分比").GetComponent<Text>().text = (int)((stage.GetScoreGet() / stage.GetScoreTotal()) * 100) + "%";
        }
    }

    /// <summary>
    /// 创建所有评分项
    /// </summary>
    /// <param name="scoreLib"></param>
    private void CreateAllItem(ScoreLibrary scoreLib, Transform trans_评分内容父类)
    {
        // 清空列表
        foreach (var item in list_当前评分项内容)
        {
            DestroyImmediate(item);
        }
        list_当前评分项内容.Clear();

        foreach (var stage in scoreLib.resultVo)
        {
            StageAdd(stage, trans_评分内容父类);
        }
    }

    /// <summary>
    /// 创建单个评分项
    /// </summary>
    /// <param name="scoreLib"></param>
    /// <param name="stageName"></param>
    private void CreateItem(ScoreLibrary scoreLib, string stageName, Transform trans_评分内容父类)
    {
        // 清空列表
        foreach (var item in list_当前评分项内容)
        {
            DestroyImmediate(item);
        }
        list_当前评分项内容.Clear();

        foreach (var stage in scoreLib.resultVo)
        {
            if (stage.stageName == stageName)
            {
                StageAdd(stage, trans_评分内容父类);
            }
        }
    }

    /// <summary>
    /// 添加得分项
    /// </summary>
    /// <param name="stage"></param>
    private void StageAdd(ScoreStageInfo stage, Transform trans_评分内容父类)
    {
        GameObject scoreItem = ObjectPoolsManager.Instance.Spawn(pre_评分项, trans_评分内容父类);
        scoreItem.transform.Find("文字项/步骤").GetComponent<Text>().text = stage.stageName;
        trans_评分小项父类 = scoreItem.transform.Find("文字项/小项组").transform;
        foreach (var item in stage.stepInfos)
        {
            StepAdd(trans_评分小项父类, item);
        }

        list_当前评分项内容.Add(scoreItem);

        LayoutRebuilder.ForceRebuildLayoutImmediate(scoreItem.GetComponent<RectTransform>());
    }

    /// <summary>
    /// 添加得分单项
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="step"></param>
    private void StepAdd(Transform parent, ScoreStepInfo step)
    {
        foreach (var point in step.pointDatas)
        {
            GameObject pointItem = ObjectPoolsManager.Instance.Spawn(pre_评分小项, parent);
            pointItem.transform.Find("评分详情").GetComponent<Text>().text = point.GetInfo();
            pointItem.transform.Find("得分").GetComponent<Text>().text = point.GetScore().ToString() + "分";
            pointItem.transform.Find("分值").GetComponent<Text>().text = point.GetTotalScore().ToString() + "分";

            //if (point.GetScore() != point.GetTotalScore())
            //{
            //    pointItem.transform.Find("评分详情").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            //    pointItem.transform.Find("得分").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            //}
            //else
            //{
            //    pointItem.transform.Find("评分详情").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
            //    pointItem.transform.Find("得分").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
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
        Transform[] objs = trans_评分内容父类.GetComponentsInChildren<Transform>();
        for (int i = 1; i < objs.Length; i++)
        {
            ObjectPoolsManager.Instance.Despawn(objs[i].gameObject);
        }
        //resultVo.details.Clear();
        //resultVo.details = new List<StepDetail>();
    }

}
