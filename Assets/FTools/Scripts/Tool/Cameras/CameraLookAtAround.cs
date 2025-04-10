
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLookAtAround : MonoBehaviour
{
    //摄像机看向的物体
    [SerializeField] private Transform target;

    private Vector3 origPos;
    private Vector3 currentPos;
    private Vector3 currentMousePos;

    //视角距离范围
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    //视角高度范围
    [SerializeField] private float minYLImit;
    [SerializeField] private float maxYLImit;

    [SerializeField] private float horizontalSpeed;  //水平滑动速度
    [SerializeField] private float verticalSpeed;    //竖直滑动速度
    [SerializeField] private float moveSpeed;        //直线移动速度
    [SerializeField] private float animSpeed;        //移动动画速度

    private bool isAnim;   //判断是否在相机移动动画中
    private Coroutine lookCor;

    public enum MouseButtonType
    {
        无按键,
        左键,
        右键,
    }
    public MouseButtonType key_移动方式 = MouseButtonType.左键;

    private void Awake()
    {
        currentPos = transform.position;
        origPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (!isAnim)
        {
            LineMove();
            AroundMove();
            PosLimit();
        }
        transform.LookAt(target);
    }

    /// <summary>
    /// 相机移动至指定位置
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(Vector3 pos)
    {
        if (lookCor != null) StopCoroutine(lookCor);
        isAnim = true;
        lookCor = StartCoroutine(MoveToTarget(pos));
    }

    /// <summary>
    /// 相机移动动画
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    IEnumerator MoveToTarget(Vector3 pos)
    {
        while (transform.localPosition != pos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, pos, animSpeed * Time.deltaTime);
            yield return null;
        }
        isAnim = false;
    }

    #region 相机移动效果
    //相机跟随鼠标移动视角
    private void AroundMove()
    {
        bool keydown = (Input.GetMouseButtonDown(0) && key_移动方式 == MouseButtonType.左键) || (Input.GetMouseButtonDown(1) && key_移动方式 == MouseButtonType.右键);
        bool keyHold = (Input.GetMouseButton(0) && key_移动方式 == MouseButtonType.左键) || (Input.GetMouseButton(1) && key_移动方式 == MouseButtonType.右键) || key_移动方式 == MouseButtonType.无按键;
        if (keydown)
        {
            currentMousePos = Input.mousePosition;
        }
        if (keyHold)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(Input.mousePosition);
            if (Input.mousePosition.x > currentMousePos.x)
            {
                transform.position -= transform.right * Time.deltaTime * horizontalSpeed;
            }
            else if (Input.mousePosition.x < currentMousePos.x)
            {
                transform.position += transform.right * Time.deltaTime * horizontalSpeed;
            }

            if (Input.mousePosition.y > currentMousePos.y)
            {
                if (transform.position.y - Time.deltaTime * verticalSpeed >= minYLImit)
                {
                    transform.position -= transform.up * Time.deltaTime * verticalSpeed;
                }
            }
            else if (Input.mousePosition.y < currentMousePos.y)
            {
                if (transform.position.y - Time.deltaTime * verticalSpeed <= maxYLImit)
                {
                    transform.position += transform.up * Time.deltaTime * verticalSpeed;
                }
            }
            currentMousePos = Input.mousePosition;
        }
    }

    //滚轮移动
    private void LineMove()
    {
        float scoll = Input.GetAxis("Mouse ScrollWheel");
        if (scoll > 0)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else if (scoll < 0)
        {
            transform.position -= transform.forward * Time.deltaTime * moveSpeed;
        }
    }

    //距离限制
    private void PosLimit()
    {
        //限制高度
        float targetY = target.position.y;

        //限制远近
        float currentDistance = Vector3.Distance(transform.position, target.position);
        if (currentDistance < maxDistance && currentDistance > minDistance)
        {
            currentPos = transform.position;
        }
        else if (currentDistance > maxDistance)
        {
            transform.position = currentPos;
            transform.position += transform.forward * Time.deltaTime;
        }
        else if (currentDistance < minDistance)
        {
            transform.position = currentPos;
            transform.position -= transform.forward * Time.deltaTime;
        }
        //transform.position = IMethod.SetVector3_Y(transform.position, Mathf.Clamp(transform.position.y, minYLImit, maxYLImit));

    }
    #endregion

    private void OnEnable()
    {
        //MoveTo(origPos);
        transform.position = origPos;
    }
}
