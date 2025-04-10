using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Tooltip("正确的物体光标")]
    [HideInInspector] public GameObject correctPoint;
    [Tooltip("正确的物体显示")]
    [HideInInspector] public GameObject correctMod;

    private Vector2 origPos;  //起始位置
    private bool isPress;     //物体UI是否被拖动
    private bool isOnPoint;   //是否在光标上
    private bool isRightObj; //是否是正确的物体UI

    //用来记录物体是否放置正确；初始为0，放置在光标上加1，放置在正确的光标上加10
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
                //物体正确且位置正确
                IsRightObj();
                IsOnRightPoint();
            }
            else if(isRightObj)
            {
                //物体正确但位置错误
                IsRightObj();
                IsOnWrongPoint();
            }
            else if(correctPoint.GetComponent<bool>() == true)
            {
                //位置错误但物体正确
                IsWrongObj();
                IsOnRightPoint();
            }
            else
            {
                //物体和位置均错误
                IsWrongObj();
                IsOnWrongPoint();
            }
        }
    }

    protected virtual void IsRightObj()
    {
        // 该物体UI正确
    }
    protected virtual void IsWrongObj()
    {
        // 该物体UI错误
    }
    protected virtual void IsOnRightPoint()
    {
        // 放在了正确光标
    }
    protected virtual void IsOnWrongPoint()
    {
        // 放在了错误光标
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
