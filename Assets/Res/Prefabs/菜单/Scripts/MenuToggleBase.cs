using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ����Menu����
/// </summary>
public abstract class MenuToggleBase : MonoBehaviour
{
    public Toggle Toggle;
    public Text Label;
    public Image Image;

    [Header("չ��")]
    public bool hasSpread;
    public SpreadedToggleManager spreadMgr;


    [HideInInspector]
    public string taskName;
    [HideInInspector]
    public int taskIndex;

    public virtual void Selected()
    {

    }
}