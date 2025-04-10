using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetSomething : MonoBehaviour
{
    [InspectorButton("��������text��ֵ")]
    private void ResetChildrensText()
    {
        for(int i = 1;i < transform.childCount;i++)
        {
            transform.GetChild(i).GetComponentInChildren<Text>().text = transform.GetChild(i).name;
        }
    }

    [InspectorButton("������text��ֵ")]
    private void ResetChildText()
    {
        GetComponentInChildren<Text>().text = name;
    }

    [InspectorButton("�Ƴ��ű�")]
    private void RemoveThis()
    {
        DestroyImmediate(this);
    }
}
