
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraLookAtAround : MonoBehaviour
{
    //��������������
    [SerializeField] private Transform target;

    private Vector3 origPos;
    private Vector3 currentPos;
    private Vector3 currentMousePos;

    //�ӽǾ��뷶Χ
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    //�ӽǸ߶ȷ�Χ
    [SerializeField] private float minYLImit;
    [SerializeField] private float maxYLImit;

    [SerializeField] private float horizontalSpeed;  //ˮƽ�����ٶ�
    [SerializeField] private float verticalSpeed;    //��ֱ�����ٶ�
    [SerializeField] private float moveSpeed;        //ֱ���ƶ��ٶ�
    [SerializeField] private float animSpeed;        //�ƶ������ٶ�

    private bool isAnim;   //�ж��Ƿ�������ƶ�������
    private Coroutine lookCor;

    public enum MouseButtonType
    {
        �ް���,
        ���,
        �Ҽ�,
    }
    public MouseButtonType key_�ƶ���ʽ = MouseButtonType.���;

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
    /// ����ƶ���ָ��λ��
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(Vector3 pos)
    {
        if (lookCor != null) StopCoroutine(lookCor);
        isAnim = true;
        lookCor = StartCoroutine(MoveToTarget(pos));
    }

    /// <summary>
    /// ����ƶ�����
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

    #region ����ƶ�Ч��
    //�����������ƶ��ӽ�
    private void AroundMove()
    {
        bool keydown = (Input.GetMouseButtonDown(0) && key_�ƶ���ʽ == MouseButtonType.���) || (Input.GetMouseButtonDown(1) && key_�ƶ���ʽ == MouseButtonType.�Ҽ�);
        bool keyHold = (Input.GetMouseButton(0) && key_�ƶ���ʽ == MouseButtonType.���) || (Input.GetMouseButton(1) && key_�ƶ���ʽ == MouseButtonType.�Ҽ�) || key_�ƶ���ʽ == MouseButtonType.�ް���;
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

    //�����ƶ�
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

    //��������
    private void PosLimit()
    {
        //���Ƹ߶�
        float targetY = target.position.y;

        //����Զ��
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
