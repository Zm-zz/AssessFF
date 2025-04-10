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
            Debug.LogWarning($"δ�ҵ�{index}������");
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

    #region ��ʱ����
    [ContextMenu("�����ܷ�")]
    void ShowTotalScore()
    {
        Debug.Log(GetScoreTotal());
    }

    /// <summary>
    /// ��ȡ�ܷ�
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
    /// ��ȡ���в������÷�
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

    //    [ContextMenu("ճ�����ȡ��������")]
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
/// ����
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
            Debug.LogWarning($"δ�ҵ�{index}������");
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
    /// ��ȡ�ò����ܷ�
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
    /// ��ȡ�ò���õ��ķ���
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
    /// ����ò����Ѿ��÷ֵĵ�
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
/// ���׶�
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
            Debug.LogWarning($"δ�ҵ�{index}������");
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
    /// ��ȡ�ý׶��ܷ�
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
    /// ��ȡ�ý׶εõ��ķ���
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
    /// ����ý׶����Ѿ��÷ֵĵ�
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
/// ÿ������
/// </summary>
[Serializable]
public class ScorePointInfo
{
    public string pointName;
    public int scoreActSet;
    public List<ScorePoint> scorePointStates = new List<ScorePoint>();

    /// <summary>
    /// ��ȡ�ܷ�
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
    /// ��ȡ�õ��ķ���
    /// </summary>
    /// <returns></returns>
    public float GetScore()
    {
        foreach (var scorePointState in scorePointStates)
        {
            if (scoreActSet == scorePointState.actIndex_��Ϊ����)
            {
                return scorePointState.scoreGet;
            }
        }
        Debug.LogWarning($"δ���{pointName}��Ӧ�ĵ÷���");
        return -100;
    }

    /// <summary>
    /// ��ȡ�÷�����
    /// </summary>
    /// <returns></returns>
    public string GetInfo()
    {
        foreach (var scorePointState in scorePointStates)
        {
            if (scoreActSet == scorePointState.actIndex_��Ϊ����)
            {
                return scorePointState.pointInfo;
            }
        }
        Debug.LogWarning($"δ���{pointName}��Ӧ�ĵ÷���");
        return null;
    }

    /// <summary>
    /// �Ƿ������
    /// </summary>
    /// <returns></returns>
    public bool GetIsCorrect()
    {
        return GetTotalScore() == GetScore();
    }

    /// <summary>
    /// �����Ѿ��÷ֵĵ�
    /// </summary>
    public void ResetScore()
    {
        scoreActSet = 0;
    }
}

/// <summary>
/// ��������
/// </summary>
[Serializable]
public class ScorePoint
{
    public string pointInfo;
    public float scoreGet;
    public int actIndex_��Ϊ����;

    public ScorePoint(string pointInfo, float score, int actIndex)
    {
        this.pointInfo = pointInfo;
        this.scoreGet = score;
        this.actIndex_��Ϊ���� = actIndex;
    }
}
