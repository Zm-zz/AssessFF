using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationJudgeExample : MonoBehaviour
{
    public ExaminationJudgeController examinationGroup;
    public string _��������;
    public bool _���õ÷�;
    public bool _�������;
    public bool _ѡ�����;
    public int _��ȡ���� = -1;

    [InspectorButton()]
    void SetExamName()
    {
        examinationGroup.SetExamination(_��������, _���õ÷�, _�������, _��ȡ����);
    }

    [InspectorButton()]
    void OnClick_�ύ()
    {
        examinationGroup.OnClick_�ύ();
    }
    [InspectorButton()]
    void OnClick_���¿���()
    {
        examinationGroup.OnClick_���¿���();
    }
}
