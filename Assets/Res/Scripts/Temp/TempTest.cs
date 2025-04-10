using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TempTest : MonoBehaviour
{
    private void Start()
    {
        Type type1 = Type.GetType("PopCenter");
        Type type2 = typeof(PopCenter);
        Debug.Log(type1);
        Debug.Log("    " + type2);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Z))
        {
            EventCenterManager.Broadcast<float, UnityAction, string, UnityAction, bool>(PopKey.PopUpLongProgressBar,
                2, () => Debug.Log(11), "Ìø¹ý", () => Debug.Log(22), true);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {

        }
    }
}
