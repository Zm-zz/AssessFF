using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationJudgeExample : MonoBehaviour
{
    public ExaminationJudgeController examinationGroup;
    public string _考题名称;
    public bool _启用得分;
    public bool _考题随机;
    public bool _选项随机;
    public int _抽取数量 = -1;

    [InspectorButton()]
    void SetExamName()
    {
        examinationGroup.SetExamination(_考题名称, _启用得分, _考题随机, _抽取数量);
    }

    [InspectorButton()]
    void OnClick_提交()
    {
        examinationGroup.OnClick_提交();
    }
    [InspectorButton()]
    void OnClick_重新考核()
    {
        examinationGroup.OnClick_重新考核();
    }
}
