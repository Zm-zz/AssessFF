using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectViewAround : MonoBehaviour
{
    public enum ViewKeyType { 左键, 右键, 中键, 无按键 }
    public ViewKeyType keyType = ViewKeyType.左键;
    public bool canView = false;

    public bool hor_水平查看;
    [ShowIf(nameof(hor_水平查看))]public float speed_水平灵敏度 = 3000;
    public bool vir_垂直查看;
    [ShowIf(nameof(vir_垂直查看))]public float speed_垂直灵敏度 = 3000;

    private void Update()
    {
        if (canView)
        {
            if ((keyType == ViewKeyType.左键 && Input.GetMouseButton(0)) ||
                (keyType == ViewKeyType.右键 && Input.GetMouseButton(1)) ||
                (keyType == ViewKeyType.中键 && Input.GetMouseButton(2)) ||
                keyType == ViewKeyType.无按键)
            {
                if (hor_水平查看 && vir_垂直查看)
                {
                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed_垂直灵敏度, -Input.GetAxis("Mouse X") * speed_水平灵敏度) * Time.deltaTime, Space.World);
                }
                else if (hor_水平查看)
                {
                    transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * speed_水平灵敏度) * Time.deltaTime, Space.World);
                }
                else if (vir_垂直查看)
                {
                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed_垂直灵敏度, 0) * Time.deltaTime, Space.World);
                }
            }
        }
    }
}
