using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class WebInteract
{
    [DllImport("__Internal")]
    public static extern void ExitFullScreen();

    [DllImport("__Internal")]
    public static extern void Reload();

    [DllImport("__Internal")]
    public static extern bool IsFullScreen();

    [DllImport("__Internal")]
    public static extern void testParamCall(string str);
}
