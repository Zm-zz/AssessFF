using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewScoreData", menuName = "FDatas/NewScoreData")]
[Serializable]
public class ScoreLibrary : ScriptableObject
{
    public string scoreName;
    public string scoreSer;
    public string scoreID;
    public List<ScoreStageInfo> resultVo = new List<ScoreStageInfo>();
    public int Length => resultVo.Count;

    public ScoreStageInfo this[string index]
    {
        get
        {
            index = index.TrimStart().TrimEnd();
            foreach (var stage in resultVo)
            {
                if (stage.stageName == index)
                {
                    return stage;
                }
            }
            Debug.LogWarning($"未找到{index}评分项");
            return null;
        }
    }

    public void ResetScore()
    {
        foreach (var stage in resultVo)
        {
            stage.ResetScore();
        }
    }
    public void ResetScore(int indexStage)
    {
        resultVo[indexStage].ResetScore();
    }
    public void ResetScore(string indexStage)
    {
        this[indexStage].ResetScore();
    }

    #region 暂时遗弃
    [ContextMenu("计算总分")]
    void ShowTotalScore()
    {
        Debug.Log(GetScoreTotal());
    }

    /// <summary>
    /// 获取总分
    /// </summary>
    /// <returns></returns>
    public float GetScoreTotal()
    {
        float score = 0;
        foreach (var stage in resultVo)
        {
            score += stage.GetScoreTotal();
        }
        return score;
    }

    /// <summary>
    /// 获取所有步骤所得分
    /// </summary>
    /// <returns></returns>
    public float GetScoreGet()
    {
        float score = 0;
        foreach (var step in resultVo)
        {
            score += step.GetScoreGet();
        }
        return score;
    }
    //public void SetActScore(int indexStep, int indexPoint, int scoreSet)
    //{
    //    resultVo[indexStep].pointDatas[indexPoint].scoreActSet = scoreSet;
    //}

    //    [ContextMenu("粘贴板获取评分数据")]
    //    void GetScoreData()
    //    {
    //        resultVo = new List<ScoreStep>();
    //        string[][] scoreData = FMethod.GetCopyExcelForm();
    //        for (int raw = 0; raw < scoreData.Length; raw++)
    //        {
    //            if (!string.IsNullOrWhiteSpace(scoreData[raw][0]))
    //            {
    //                ScoreStep step = new ScoreStep();
    //                step.stepName = scoreData[raw][0];
    //                for (int i = raw; i < scoreData.Length; i++)
    //                {
    //                    if (!string.IsNullOrWhiteSpace(scoreData[i][0]) && i != raw)
    //                    {
    //                        break;
    //                    }
    //                    if (!string.IsNullOrWhiteSpace(scoreData[i][1]))
    //                    {
    //                        ScorePointData point = new ScorePointData();
    //                        point.pointName = scoreData[i][1];
    //                        point.scorePointStates.Add(new ScorePointState(scoreData[i][2], float.Parse(scoreData[i][3]), int.Parse(scoreData[i][4])));
    //                        for (int j = i + 1; j < scoreData.Length; j++)
    //                        {
    //                            if (!string.IsNullOrWhiteSpace(scoreData[j][1]))
    //                            {
    //                                break;
    //                            }
    //                            point.scorePointStates.Add(new ScorePointState(scoreData[j][2], float.Parse(scoreData[j][3]), int.Parse(scoreData[j][4])));
    //                        }
    //                        step.pointDatas.Add(point);
    //                    }
    //                }
    //                resultVo.Add(step);
    //            }
    //        }
    //#if UNITY_EDITOR
    //        UnityEditor.EditorUtility.SetDirty(this);
    //#endif
    #endregion
}

/// <summary>
/// 大步骤
/// </summary>
[Serializable]
public class ScoreStageInfo
{
    public string stageName;
    public int Length => stepInfos.Count;
    public List<ScoreStepInfo> stepInfos = new List<ScoreStepInfo>();

    public ScoreStepInfo this[string index]
    {
        get
        {
            index = index.TrimStart().TrimEnd();
            foreach (var step in stepInfos)
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

    public bool GetIsDone()
    {
        bool done = true;
        foreach (var step in stepInfos)
        {
            done &= step.GetIsDone();
        }
        return done;
    }

    /// <summary>
    /// 获取该步骤总分
    /// </summary>
    /// <returns></returns>
    public float GetScoreTotal()
    {
        float score = 0;
        foreach (var step in stepInfos)
        {
            score += step.GetScoreTotal();
        }
        return score;
    }

    /// <summary>
    /// 获取该步骤得到的分数
    /// </summary>
    /// <returns></returns>
    public float GetScoreGet()
    {
        float score = 0;
        foreach (var step in stepInfos)
        {
            score += step.GetScoreGet();
        }
        return score;
    }

    /// <summary>
    /// 重设该步骤已经得分的点
    /// </summary>
    public void ResetScore()
    {
        foreach (var step in stepInfos)
        {
            step.ResetScore();
        }
    }
}

/// <summary>
/// 各阶段
/// </summary>
[Serializable]
public class ScoreStepInfo
{
    public string stepName;
    public int Length => pointDatas.Count;
    public List<ScorePointInfo> pointDatas = new List<ScorePointInfo>();
    public int this[int index]
    {
        get { return pointDatas[index].scoreActSet; }
        set { pointDatas[index].scoreActSet = value; }
    }
    public ScorePointInfo this[string index]
    {
        get
        {
            index = index.TrimStart().TrimEnd();
            foreach (var point in pointDatas)
            {
                if (point.pointName == index)
                {
                    return point;
                }
            }
            Debug.LogWarning($"未找到{index}评分项");
            return null;
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

    /// <summary>
    /// 获取该阶段总分
    /// </summary>
    /// <returns></returns>
    public float GetScoreTotal()
    {
        float score = 0;
        foreach (var point in pointDatas)
        {
            score += point.GetTotalScore();
        }
        return score;
    }

    /// <summary>
    /// 获取该阶段得到的分数
    /// </summary>
    /// <returns></returns>
    public float GetScoreGet()
    {
        float score = 0;
        foreach (var point in pointDatas)
        {
            score += point.GetScore();
        }
        return score;
    }

    /// <summary>
    /// 重设该阶段内已经得分的点
    /// </summary>
    public void ResetScore()
    {
        foreach (var point in pointDatas)
        {
            point.ResetScore();
        }
    }
}

/// <summary>
/// 每个单点
/// </summary>
[Serializable]
public class ScorePointInfo
{
    public string pointName;
    public int scoreActSet;
    public List<ScorePoint> scorePointStates = new List<ScorePoint>();

    /// <summary>
    /// 获取总分
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 获取得到的分数
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 获取得分详情
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 是否地满分
    /// </summary>
    /// <returns></returns>
    public bool GetIsCorrect()
    {
        return GetTotalScore() == GetScore();
    }

    /// <summary>
    /// 重设已经得分的点
    /// </summary>
    public void ResetScore()
    {
        scoreActSet = 0;
    }
}

/// <summary>
/// 单点内容
/// </summary>
[Serializable]
public class ScorePoint
{
    public string pointInfo;
    public float scoreGet;
    public int actIndex_行为索引;

    public ScorePoint(string pointInfo, float score, int actIndex)
    {
        this.pointInfo = pointInfo;
        this.scoreGet = score;
        this.actIndex_行为索引 = actIndex;
    }
}
