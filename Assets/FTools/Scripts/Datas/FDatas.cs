using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FDatas
{
    public static TaskType taskType;
    public static int curIndex_�������;
}

public enum TaskType { ѵ��ģʽ,����ģʽ}
public enum GameCommand { ��ͣ,ȡ����ͣ,����}
public enum TaskCommand 
{ 
    Init,Init_Before,Init_After,
    Reset,Reset_Before,Reset_After,
    Stop,Stop_Before,Stop_After,
    Enter,Enter_Before,Enter_After,
    Exit,Exit_Before,Exit_After,
}