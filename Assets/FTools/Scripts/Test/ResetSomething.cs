using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetSomething : MonoBehaviour
{
    [InspectorButton("给孩子们text赋值")]
    private void ResetChildrensText()
    {
        for(int i = 1;i < transform.childCount;i++)
        {
            transform.GetChild(i).GetComponentInChildren<Text>().text = transform.GetChild(i).name;
        }
    }

    [InspectorButton("给孩子text赋值")]
    private void ResetChildText()
    {
        GetComponentInChildren<Text>().text = name;
    }

    [InspectorButton("移除脚本")]
    private void RemoveThis()
    {
        DestroyImmediate(this);
    }
}
