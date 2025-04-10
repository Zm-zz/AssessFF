using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Tooltip("��ȷ��������")]
    [HideInInspector] public GameObject correctPoint;
    [Tooltip("��ȷ��������ʾ")]
    [HideInInspector] public GameObject correctMod;

    private Vector2 origPos;  //��ʼλ��
    private bool isPress;     //����UI�Ƿ��϶�
    private bool isOnPoint;   //�Ƿ��ڹ����
    private bool isRightObj; //�Ƿ�����ȷ������UI

    //������¼�����Ƿ������ȷ����ʼΪ0�������ڹ���ϼ�1����������ȷ�Ĺ���ϼ�10
    [HideInInspector] public static int correctHandCount { set; get; }

    private void Awake()
    {
        origPos = transform.position;
        isOnPoint = false;
        isPress = false;
    }

    public void Init(GameObject correctPoint = null, GameObject correctMod = null, bool isRightObj = false)
    {
        gameObject.SetActive(true);
        transform.localPosition = origPos;
        isPress = false;
        this.correctPoint = correctPoint;
        this.correctMod = correctMod;
        this.isRightObj = isRightObj;
        correctHandCount = 0;
    }

    private void Update()
    {
        isOnPoint = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitting = Physics.Raycast(ray, out hit, 9999, LayerMask.GetMask("PointTrigger"));
        if (hitting && isPress)
        {
            isOnPoint = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        origPos = transform.localPosition;
        isPress = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localPosition = origPos;
        isPress = false;

        if (isOnPoint)
        {
            if (correctPoint.GetComponent<bool>() == true && isRightObj)
            {
                //������ȷ��λ����ȷ
                IsRightObj();
                IsOnRightPoint();
            }
            else if(isRightObj)
            {
                //������ȷ��λ�ô���
                IsRightObj();
                IsOnWrongPoint();
            }
            else if(correctPoint.GetComponent<bool>() == true)
            {
                //λ�ô���������ȷ
                IsWrongObj();
                IsOnRightPoint();
            }
            else
            {
                //�����λ�þ�����
                IsWrongObj();
                IsOnWrongPoint();
            }
        }
    }

    protected virtual void IsRightObj()
    {
        // ������UI��ȷ
    }
    protected virtual void IsWrongObj()
    {
        // ������UI����
    }
    protected virtual void IsOnRightPoint()
    {
        // ��������ȷ���
    }
    protected virtual void IsOnWrongPoint()
    {
        // �����˴�����
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }

    private void OnEnable()
    {
        origPos = transform.localPosition;
    }
}
