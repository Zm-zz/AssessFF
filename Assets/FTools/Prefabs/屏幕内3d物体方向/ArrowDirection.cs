using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowDirection : MonoBehaviour
{
    Vector3 start => transform.Find("start").position;
    Vector3 end => transform.Find("end").position;

    public Vector3 GetDirInScene(bool normalized = true)
    {
        return normalized ? (start - end).normalized : (start - end);
    }

    public Vector3 GetDirInScreen(bool normalized = true)
    {
        Vector3 ss = Camera.main.WorldToScreenPoint(start);
        Vector3 ee = Camera.main.WorldToScreenPoint(end);
        return normalized ? (ss - ee).normalized : (ss - ee);
    }

    public bool IsOnDir(Vector3 dir)
    {
        return DirSpeed(dir) > 0;
    }

    public float DirSpeed(Vector3 dir)   //0-1
    {
        return Vector3.Dot(dir.normalized, GetDirInScreen(true));
    }
}
