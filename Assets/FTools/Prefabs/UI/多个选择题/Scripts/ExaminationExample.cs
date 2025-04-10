using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationExample : MonoBehaviour
{
    public ExaminationGroupController examinationGroup;
    public string _��������;
    public bool _���õ÷�;
    public bool _�������;
    public bool _ѡ�����;
    public int _��ȡ���� = -1;

    [InspectorButton()]
    public void SetExam()
    {
        List<ExaminationInfo> examinations = new List<ExaminationInfo>
        {
            new ExaminationInfo(
            "���׺���������Ҫʹ��Ŀ����ʲô��",
            new string[] { "A. ά�ֺ����ӻ���ͨ����", "B. ������в�����ĵ���Ѫ֢", "C. ������������г���ͨ��", "D. �������ķθ���" },
            "ab",0,true
        ),
            new ExaminationInfo(
            "���׺�������������Щ�����",
            new string[] { "A. ����ֹͣ", "B. ����˥������", "C. ����������", "D. ���Ͳ�Ա" },
            "abcd"
        ),
            new ExaminationInfo(
            "���׺��������������������ʲô��",
            new string[] { "A. ����", "B. ����", "C. ͨ����", "D. ����������" },
            "c"
        ),
            new ExaminationInfo(
            "��ѹ������ʱ��ӦԼ��ѹ�����ҵĶ���Ϊ�ˣ�",
            new string[] { "A. 1/3��2/3", "B. 1/3��1/2", "C. 1/3��3/4", "D. 1/2��3/4" },
            "a"
        ),
            new ExaminationInfo(
            "���׺�����������ʱ����������",
            new string[] { "A. ��һ��ʹ������ʱ", "B. ͬһ����ʹ�ó���24Сʱ", "C. ͬһ����ʹ��δ����48Сʱ", "D. ��ͬ����ʹ��ʱ" },
            "c"
        ),
            new ExaminationInfo(
            "�������ͨ����ʱ��Ӧ����β�����",
            new string[] { "A. �乭���ϲ嵽�����", "B. �乭���²嵽�������ת��180��", "C. ֱ�Ӳ����ǻ����", "D. �����������" },
            "b"
        ),
            new ExaminationInfo(
            "���׺������ļ��Ƶ��ӦΪ���һ�Σ�",
            new string[] { "A. ÿ��", "B. ÿ��", "C. ÿ��", "D. ÿ����" },
            "b"
        ),
            new ExaminationInfo(
            "���׺�������������������У�����������������ȷ�ģ�",
            new string[] { "A. �����������ʹ�þƾ�����", "B. �����������ʹ�����ȩ����", "C. ���������Ҫ������෽��", "D. �������ǰ�����ж�����" },
            "b"
        ),
            new ExaminationInfo(
            "���׺�������������ʱ��������Ӧ���������٣�",
            new string[] { "A. 4-6��/��", "B. 6-8��/��", "C. 8-10��/��", "D. 10-12��/��" },
            "c"
        ),
            new ExaminationInfo(
            "ʹ�ü��׺�����ʱ��ÿ��������ӦΪ���٣�",
            new string[] { "A. �̶�ֵ�����ܻ������Ӱ��", "B. ���ݻ������������", "C. Խ��Խ�ã��Ա�֤ͨ����", "D. Խ��Խ�ã��������˷���֯" },
            "b"
        )
        };
        examinationGroup.SetExamination(examinations, _���õ÷�, _�������, _ѡ�����, _��ȡ����);
    }

    [InspectorButton()]
    public void SetExamName()
    {
        examinationGroup.SetExamination(_��������, _���õ÷�, _�������, _ѡ�����, _��ȡ����);
    }
}
