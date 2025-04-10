using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 聚焦相机，挂载在对应的RawImage上
/// </summary>
public class FocusCamera : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    bool isIn;
    public Camera lookCamera;
    public float moveSpeed;
    public float deltaSpeedX;
    public float deltaSpeedY;
    public float maxDistance;
    Vector3 originPos;

    private void Awake()
    {
        originPos = lookCamera.transform.position;
    }

    private void Update()
    {
        float deltaX = Input.mousePosition.x - transform.position.x;
        float deltaY = Input.mousePosition.y - transform.position.y;
        Vector3 lookPos = lookCamera.transform.forward + lookCamera.transform.up * deltaY * deltaSpeedY + lookCamera.transform.right * deltaX * deltaSpeedX ;
        if (isIn)
        {
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                lookCamera.transform.Translate((originPos - lookCamera.transform.position).normalized * Time.deltaTime * moveSpeed, Space.World);
                if (Vector3.Dot(lookCamera.transform.forward, (originPos - lookCamera.transform.position).normalized) >= 0)
                {
                    lookCamera.transform.position = originPos;
                }
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                if (FMethod.Cos(Vector3.Angle(lookCamera.transform.position - originPos, lookCamera.transform.forward)) * (Vector3.Distance(originPos, lookCamera.transform.position)) >= maxDistance)
                    return;
                lookCamera.transform.Translate(lookPos.normalized * Time.deltaTime * moveSpeed, Space.World);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
    }
}
