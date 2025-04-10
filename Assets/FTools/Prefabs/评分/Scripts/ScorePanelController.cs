using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScorePanelController : SingletonPatternMonoBase<ScorePanelController>
{
    public ScoreItemLibrary scoreLibrary;
    [Space]
    public bool is_是否需要上传成绩;
    public GameObject scorePanel;
    public Transform scoreItemBigParent;
    public GameObject scoreItemBigPrefab;
    public GameObject scoreItemLittlePrefab;
    public Button btn_完成按钮;
    Transform scoreItemLittleParent;
    
    [Header("分数")]
    public Image scoreImg;
    public Sprite goodSprite;
    public Sprite badSprite;
    public Text scoreText;

    ResultVo resultVo;
    private void Awake()
    {
        if (is_是否需要上传成绩)
        {
            BackendData data = new BackendData();
            data.IsStandalone = false;
            data.AppID = "S-000281";                //S-000216，记得填
            data.AppProtocol = "UGION000281";          //UGION000216，记得填
            data.AppVersion = "1.0";
            Backend.Instance.Init(data);
        }
        resultVo = new ResultVo();
        resultVo.details = new List<StepDetail>();

        btn_完成按钮.onClick.AddListener(OnClick_完成);
    }

    public void SubmitScore()
    {
        ClearScore();
        SetScore();

        if (is_是否需要上传成绩)
        {
            ////////////////Backend.Instance.BeginTrain(isTrain);!!!!!!!!!!!!!
            ////////////////需要指定考核模式或训练模式
            resultVo.totalScore = scoreLibrary.GetScoreTotal();
            resultVo.moduleLabel = scoreLibrary.scoreID;
            Backend.Instance.Submit(resultVo);
        }

    }

    public void BeginTrain(bool isTrain)
    {
        if (is_是否需要上传成绩)
        {
            Backend.Instance.BeginTrain(isTrain);
        }
    }
    public void SetID(string id)
    {
        resultVo.moduleLabel = id;
    }

    public void ShowScore()
    {
        scorePanel.SetActive(true);
        SubmitScore();
        scoreText.text = scoreLibrary.GetScoreGet().ToString();
        scoreImg.sprite = scoreLibrary.GetScoreGet() >= scoreLibrary.GetScoreTotal()*0.6f ? goodSprite : badSprite;
    }

    void SetScore()
    {
        foreach(var step in scoreLibrary.resultVo)
        {
            BigScoreAdd(step);
        }
    }

    void OnClick_完成()
    {
        scorePanel.SetActive(false);
    }

    private void BigScoreAdd(ScoreStep step)
    {
        GameObject scoreItem = ObjectPoolsManager.Instance.Spawn(scoreItemBigPrefab, scoreItemBigParent);
        scoreItem.transform.Find("文字项/步骤").GetComponent<Text>().text = step.stepName;
        scoreItemLittleParent = scoreItem.transform.Find("文字项/小项组").transform;
        foreach (var item in step.pointDatas)
        {
            LittleScoreAdd(scoreItemLittleParent, item);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(scoreItem.GetComponent<RectTransform>());

        StepDetail stepDetail = new StepDetail();
        stepDetail.pointDetails = new List<PointDetail>();
        stepDetail.done = step.GetIsDone();
        foreach (var item in step.pointDatas)
        {
            stepDetail.pointDetails.Add(SubmitPoint(item));
        }
        resultVo.details.Add(stepDetail);
    }

    private void LittleScoreAdd(Transform parent, ScorePointData point)
    {
        GameObject littleScoreItem = ObjectPoolsManager.Instance.Spawn(scoreItemLittlePrefab, parent);
        littleScoreItem.transform.Find("评分详情").GetComponent<Text>().text = point.GetInfo();
        littleScoreItem.transform.Find("得分").GetComponent<Text>().text = point.GetScore().ToString();
        littleScoreItem.transform.Find("分值").GetComponent<Text>().text = point.GetTotalScore().ToString();
        if (point.GetScore() != point.GetTotalScore())
        {
            littleScoreItem.transform.Find("评分详情").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            littleScoreItem.transform.Find("得分").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
        }
        else
        {
            littleScoreItem.transform.Find("评分详情").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
            littleScoreItem.transform.Find("得分").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(littleScoreItem.GetComponent<RectTransform>());
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
        Transform[] objs = scoreItemBigParent.GetComponentsInChildren<Transform>();
        for (int i = 1; i < objs.Length; i++)
        {
            ObjectPoolsManager.Instance.Despawn(objs[i].gameObject);
        }
        resultVo.details.Clear();
        resultVo.details = new List<StepDetail>();
    }

    enum ItemState { 正确,错误,未做}
}
