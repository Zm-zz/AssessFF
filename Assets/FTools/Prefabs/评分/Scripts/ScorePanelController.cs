using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScorePanelController : SingletonPatternMonoBase<ScorePanelController>
{
    public ScoreItemLibrary scoreLibrary;
    [Space]
    public bool is_�Ƿ���Ҫ�ϴ��ɼ�;
    public GameObject scorePanel;
    public Transform scoreItemBigParent;
    public GameObject scoreItemBigPrefab;
    public GameObject scoreItemLittlePrefab;
    public Button btn_��ɰ�ť;
    Transform scoreItemLittleParent;
    
    [Header("����")]
    public Image scoreImg;
    public Sprite goodSprite;
    public Sprite badSprite;
    public Text scoreText;

    ResultVo resultVo;
    private void Awake()
    {
        if (is_�Ƿ���Ҫ�ϴ��ɼ�)
        {
            BackendData data = new BackendData();
            data.IsStandalone = false;
            data.AppID = "S-000281";                //S-000216���ǵ���
            data.AppProtocol = "UGION000281";          //UGION000216���ǵ���
            data.AppVersion = "1.0";
            Backend.Instance.Init(data);
        }
        resultVo = new ResultVo();
        resultVo.details = new List<StepDetail>();

        btn_��ɰ�ť.onClick.AddListener(OnClick_���);
    }

    public void SubmitScore()
    {
        ClearScore();
        SetScore();

        if (is_�Ƿ���Ҫ�ϴ��ɼ�)
        {
            ////////////////Backend.Instance.BeginTrain(isTrain);!!!!!!!!!!!!!
            ////////////////��Ҫָ������ģʽ��ѵ��ģʽ
            resultVo.totalScore = scoreLibrary.GetScoreTotal();
            resultVo.moduleLabel = scoreLibrary.scoreID;
            Backend.Instance.Submit(resultVo);
        }

    }

    public void BeginTrain(bool isTrain)
    {
        if (is_�Ƿ���Ҫ�ϴ��ɼ�)
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

    void OnClick_���()
    {
        scorePanel.SetActive(false);
    }

    private void BigScoreAdd(ScoreStep step)
    {
        GameObject scoreItem = ObjectPoolsManager.Instance.Spawn(scoreItemBigPrefab, scoreItemBigParent);
        scoreItem.transform.Find("������/����").GetComponent<Text>().text = step.stepName;
        scoreItemLittleParent = scoreItem.transform.Find("������/С����").transform;
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
        littleScoreItem.transform.Find("��������").GetComponent<Text>().text = point.GetInfo();
        littleScoreItem.transform.Find("�÷�").GetComponent<Text>().text = point.GetScore().ToString();
        littleScoreItem.transform.Find("��ֵ").GetComponent<Text>().text = point.GetTotalScore().ToString();
        if (point.GetScore() != point.GetTotalScore())
        {
            littleScoreItem.transform.Find("��������").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
            littleScoreItem.transform.Find("�÷�").GetComponent<Text>().color = new Color(228.0f / 255, 62.0f / 255, 62.0f / 255, 1);
        }
        else
        {
            littleScoreItem.transform.Find("��������").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
            littleScoreItem.transform.Find("�÷�").GetComponent<Text>().color = new Color(96.0f / 255, 190.0f / 255, 179.0f / 255, 1);
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

    enum ItemState { ��ȷ,����,δ��}
}
