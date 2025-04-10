using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler,IDragHandler, IInitializePotentialDragHandler
{
    public bool resetOrigin = true;

    bool canDrag;
    Vector3 origPos;
    Vector3 offset;
    Vector3 dragOriginPos;
    Action downAction; 
    Action upAction;

    private void Awake()
    {
        origPos = transform.position;
    }

    public void CanDrag(bool canDrag,Action downAction = null,Action upAction = null)
    {
        this.canDrag = canDrag;
        this.downAction = downAction;
        this.upAction = upAction;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (canDrag)
        {
            dragOriginPos = Camera.main.WorldToScreenPoint(transform.position);
            offset = Camera.main.WorldToScreenPoint(transform.position) - new Vector3(Input.mousePosition.x,Input.mousePosition.y, dragOriginPos.z);
            Debug.Log(offset);
            downAction?.Invoke();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(resetOrigin)
        {
            transform.position = origPos;
        }
        if(canDrag)
        {
            upAction?.Invoke();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(canDrag)
        {
            Vector3 mousePoint = Input.mousePosition;
            mousePoint.z = dragOriginPos.z; // 保持屏幕深度的z值不变，以避免因深度变化导致的抖动  
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(mousePoint + offset);

            transform.localPosition = new Vector3(worldPoint.x, worldPoint.y, worldPoint.z);
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }
}
