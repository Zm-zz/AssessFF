using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarReset : MonoBehaviour
{
    [Range(0, 1.0f)] public float value;
    private void OnEnable()
    {
        if (GetComponent<Scrollbar>())
        {
            GetComponent<Scrollbar>().value = value;
        }
        else
        {
            Debug.LogWarning("未获取到ScrollBar组件");
        }
    }
}
