using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExaminationExample : MonoBehaviour
{
    public ExaminationGroupController examinationGroup;
    public string _考题名称;
    public bool _启用得分;
    public bool _考题随机;
    public bool _选项随机;
    public int _抽取数量 = -1;

    [InspectorButton()]
    public void SetExam()
    {
        List<ExaminationInfo> examinations = new List<ExaminationInfo>
        {
            new ExaminationInfo(
            "简易呼吸器的主要使用目的是什么？",
            new string[] { "A. 维持和增加机体通气量", "B. 纠正威胁生命的低氧血症", "C. 代替呼吸机进行长期通气", "D. 仅用于心肺复苏" },
            "ab",0,true
        ),
            new ExaminationInfo(
            "简易呼吸器适用于哪些情况？",
            new string[] { "A. 呼吸停止", "B. 呼吸衰竭抢救", "C. 呼吸机故障", "D. 运送病员" },
            "abcd"
        ),
            new ExaminationInfo(
            "简易呼吸器的连接组件不包括什么？",
            new string[] { "A. 面罩", "B. 单向阀", "C. 通气阀", "D. 氧气储气袋" },
            "c"
        ),
            new ExaminationInfo(
            "挤压呼吸囊时，应约挤压呼吸囊的多少为宜？",
            new string[] { "A. 1/3～2/3", "B. 1/3～1/2", "C. 1/3～3/4", "D. 1/2～3/4" },
            "a"
        ),
            new ExaminationInfo(
            "简易呼吸器消毒的时机不包括？",
            new string[] { "A. 第一次使用新球时", "B. 同一患者使用超过24小时", "C. 同一患者使用未超过48小时", "D. 不同患者使用时" },
            "c"
        ),
            new ExaminationInfo(
            "插入口咽通气管时，应该如何操作？",
            new string[] { "A. 弯弓向上插到舌根部", "B. 弯弓向下插到舌根部再转动180度", "C. 直接插入口腔中央", "D. 无需特殊操作" },
            "b"
        ),
            new ExaminationInfo(
            "简易呼吸器的检测频率应为多久一次？",
            new string[] { "A. 每天", "B. 每周", "C. 每月", "D. 每季度" },
            "b"
        ),
            new ExaminationInfo(
            "简易呼吸器的清洁消毒过程中，以下哪项描述是正确的？",
            new string[] { "A. 所有配件均可使用酒精消毒", "B. 所有配件均可使用戊二醛消毒", "C. 部分配件需要特殊清洁方法", "D. 清洁消毒前无需拆卸各组件" },
            "b"
        ),
            new ExaminationInfo(
            "简易呼吸器连接氧气时，氧流量应调节至多少？",
            new string[] { "A. 4-6升/分", "B. 6-8升/分", "C. 8-10升/分", "D. 10-12升/分" },
            "c"
        ),
            new ExaminationInfo(
            "使用简易呼吸器时，每次送气量应为多少？",
            new string[] { "A. 固定值，不受患者情况影响", "B. 根据患者情况灵活调整", "C. 越多越好，以保证通气量", "D. 越少越好，以免损伤肺组织" },
            "b"
        )
        };
        examinationGroup.SetExamination(examinations, _启用得分, _考题随机, _选项随机, _抽取数量);
    }

    [InspectorButton()]
    public void SetExamName()
    {
        examinationGroup.SetExamination(_考题名称, _启用得分, _考题随机, _选项随机, _抽取数量);
    }
}
