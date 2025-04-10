using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskController : MonoBehaviour
{
    static Image mask;

    private void Awake()
    {
        mask = GetComponent<Image>();
        HideMask();
    }

    public static void SetMask(float degree = 0.1f)
    {
        mask.enabled = true;
        mask.color = FMethod.SetColor_A(mask.color, degree);
    }

    public static void HideMask()
    {
        mask.enabled = false;
    }
}
