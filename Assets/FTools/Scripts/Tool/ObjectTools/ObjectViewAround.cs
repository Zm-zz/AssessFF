using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectViewAround : MonoBehaviour
{
    public enum ViewKeyType { ���, �Ҽ�, �м�, �ް��� }
    public ViewKeyType keyType = ViewKeyType.���;
    public bool canView = false;

    public bool hor_ˮƽ�鿴;
    [ShowIf(nameof(hor_ˮƽ�鿴))]public float speed_ˮƽ������ = 3000;
    public bool vir_��ֱ�鿴;
    [ShowIf(nameof(vir_��ֱ�鿴))]public float speed_��ֱ������ = 3000;

    private void Update()
    {
        if (canView)
        {
            if ((keyType == ViewKeyType.��� && Input.GetMouseButton(0)) ||
                (keyType == ViewKeyType.�Ҽ� && Input.GetMouseButton(1)) ||
                (keyType == ViewKeyType.�м� && Input.GetMouseButton(2)) ||
                keyType == ViewKeyType.�ް���)
            {
                if (hor_ˮƽ�鿴 && vir_��ֱ�鿴)
                {
                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed_��ֱ������, -Input.GetAxis("Mouse X") * speed_ˮƽ������) * Time.deltaTime, Space.World);
                }
                else if (hor_ˮƽ�鿴)
                {
                    transform.Rotate(new Vector3(0, -Input.GetAxis("Mouse X") * speed_ˮƽ������) * Time.deltaTime, Space.World);
                }
                else if (vir_��ֱ�鿴)
                {
                    transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed_��ֱ������, 0) * Time.deltaTime, Space.World);
                }
            }
        }
    }
}
