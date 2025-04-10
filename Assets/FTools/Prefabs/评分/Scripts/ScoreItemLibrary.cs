using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ScoreData", menuName = "FDatas/ScoreData")]
[Serializable]
public class ScoreItemLibrary : ScriptableObject
{
    public string scoreName;
    public string scoreSer;
    public string scoreID;
    public List<ScoreStep> resultVo = new List<ScoreStep>();
    public int Length => resultVo.Count;
    public ScoreStep this[int index]
    {
        get { return resultVo[index]; }
    }
    public ScoreStep this[string index]
    {
        get
        {
            index = index.TrimStart().TrimEnd();
            foreach (var step in resultVo)
            {
                if (step.stepName == index)
                {
                    return step;
                }
            }
            Debug.LogWarning($"未找到{index}评分项");
            return null;
        }
    }

    [ContextMenu("计算总分")]
    void ShowTotalScore()
    {
        Debug.Log(GetScoreTotal());
    }
    public float GetScoreTotal()
    {
        float score = 0;
        foreach (var step in resultVo)
        {
            score += step.GetScoreTotal();
        }
        return score;
    }
    public float GetScoreGet()
    {
        float score = 0;
        foreach (var step in resultVo)
        {
            score += step.GetScoreGet();
        }
        return score;
    }
    public void SetActScore(int indexStep, int indexPoint, int scoreSet)
    {
        resultVo[indexStep].pointDatas[indexPoint].scoreActSet = scoreSet;
    }
    public void ResetScore()
    {
        foreach (var step in resultVo)
        {
            step.ResetScore();
        }
    }
    public void ResetScore(int indexStep)
    {
        resultVo[indexStep].ResetScore();
    }
    public void ResetScore(string indexStep)
    {
        this[indexStep].ResetScore();
    }


    [ContextMenu("粘贴板获取评分数据")]
    void GetScoreData()
    {
        resultVo = new List<ScoreStep>();
        string[][] scoreData = FMethod.GetCopyExcelForm();
        for (int raw = 0; raw < scoreData.Length; raw++)
        {
            if (!string.IsNullOrWhiteSpace(scoreData[raw][0]))
            {
                ScoreStep step = new ScoreStep();
                step.stepName = scoreData[raw][0];
                for (int i = raw; i < scoreData.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(scoreData[i][0]) && i != raw)
                    {
                        break;
                    }
                    if (!string.IsNullOrWhiteSpace(scoreData[i][1]))
                    {
                        ScorePointData point = new ScorePointData();
                        point.pointName = scoreData[i][1];
                        point.scorePointStates.Add(new ScorePointState(scoreData[i][2], float.Parse(scoreData[i][3]), int.Parse(scoreData[i][4])));
                        for (int j = i + 1; j < scoreData.Length; j++)
                        {
                            if (!string.IsNullOrWhiteSpace(scoreData[j][1]))
                            {
                                break;
                            }
                            point.scorePointStates.Add(new ScorePointState(scoreData[j][2], float.Parse(scoreData[j][3]), int.Parse(scoreData[j][4])));
                        }
                        step.pointDatas.Add(point);
                    }
                }
                resultVo.Add(step);
            }
        }
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

[Serializable]
public class ScoreStep
{
    public string stepName;
    public int Length => pointDatas.Count;
    public List<ScorePointData> pointDatas = new List<ScorePointData>();
    public int this[int index]
    {
        get { return pointDatas[index].scoreActSet; }
        set { pointDatas[index].scoreActSet = value; }
    }
    public int this[string index]
    {
        get
        {
            index = index.TrimStart().TrimEnd();
            foreach (var point in pointDatas)
            {
                if (point.pointName == index)
                {
                    return point.scoreActSet;
                }
            }
            Debug.LogWarning($"未找到{index}评分项");
            return -100;
        }
        set
        {
            index = index.TrimStart().TrimEnd();
            foreach (var point in pointDatas)
            {
                if (point.pointName == index)
                {
                    point.scoreActSet = value;
                    return;
                }
            }
            Debug.LogWarning($"未找到{index}评分项");
            return;
        }
    }

    public bool GetIsDone()
    {
        bool done = true;
        foreach (var point in pointDatas)
        {
            done &= point.GetIsCorrect();
        }
        return done;
    }
    public float GetScoreTotal()
    {
        float score = 0;
        foreach (var point in pointDatas)
        {
            score += point.GetTotalScore();
        }
        return score;
    }
    public float GetScoreGet()
    {
        float score = 0;
        foreach (var point in pointDatas)
        {
            score += point.GetScore();
        }
        return score;
    }
    public void ResetScore()
    {
        foreach (var point in pointDatas)
        {
            point.ResetScore();
        }
    }
}

[Serializable]
public class ScorePointData
{
    public string pointName;
    public int scoreActSet;
    public List<ScorePointState> scorePointStates = new List<ScorePointState>();

    public float GetTotalScore()
    {
        float totalScore = 0;
        foreach (var point in scorePointStates)
        {
            if (point.scoreGet > totalScore)
            {
                totalScore = point.scoreGet;
            }
        }
        return totalScore;
    }
    public float GetScore()
    {
        foreach (var scorePointState in scorePointStates)
        {
            if (scoreActSet == scorePointState.actIndex_行为索引)
            {
                return scorePointState.scoreGet;
            }
        }
        Debug.LogWarning($"未获得{pointName}对应的得分项");
        return -100;
    }
    public string GetInfo()
    {
        foreach (var scorePointState in scorePointStates)
        {
            if (scoreActSet == scorePointState.actIndex_行为索引)
            {
                return scorePointState.pointInfo;
            }
        }
        Debug.LogWarning($"未获得{pointName}对应的得分项");
        return null;
    }
    public bool GetIsCorrect()
    {
        return GetTotalScore() == GetScore();
    }
    public void ResetScore()
    {
        scoreActSet = 0;
    }
}

[Serializable]
public class ScorePointState
{
    public string pointInfo;
    public float scoreGet;
    public int actIndex_行为索引;

    public ScorePointState(string pointInfo, float score, int actIndex)
    {
        this.pointInfo = pointInfo;
        this.scoreGet = score;
        this.actIndex_行为索引 = actIndex;
    }
}