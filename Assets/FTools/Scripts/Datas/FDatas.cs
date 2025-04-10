using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class FDatas
{
    public static TaskType taskType;
    public static int curIndex_流程序号;
}

public enum TaskType { 训练模式,考核模式}
public enum GameCommand { 暂停,取消暂停,加速}
public enum TaskCommand 
{ 
    Init,Init_Before,Init_After,
    Reset,Reset_Before,Reset_After,
    Stop,Stop_Before,Stop_After,
    Enter,Enter_Before,Enter_After,
    Exit,Exit_Before,Exit_After,
}