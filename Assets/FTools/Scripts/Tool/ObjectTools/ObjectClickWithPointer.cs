using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectClickWithPointer : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerUpHandler
{
    public bool enable = true;
    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseUp;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;
    public bool isIn { get; private set; }

    protected virtual void Awake()
    {
        if (!Camera.main.GetComponent<PhysicsRaycaster>())
        {
            Camera.main.GetComponent<PhysicsRaycaster>();
        }
        OnMouseDown.AddListener(() => { Down(); });
        OnMouseUp.AddListener(() => { Up(); });
        OnMouseEnter.AddListener(() => { Enter(); });
        OnMouseExit.AddListener(() => { Exit(); });
    }

    private void Update()
    {
        if (!enable)
        {
            isIn = false;
            return;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!enable) return;
        OnMouseDown?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enable) return;
        isIn = true;
        OnMouseEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enable) return;
        isIn = false;
        OnMouseExit?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!enable) return;     
        OnMouseUp?.Invoke();
    }

    protected virtual void Enter()
    {
        //�����¼�
    }

    protected virtual void Exit()
    {
        //�Ƴ��¼�
    }

    protected virtual void Down()
    {
        //�����¼�
    }

    protected virtual void Up()
    {
        //̧���¼�
    }
}
