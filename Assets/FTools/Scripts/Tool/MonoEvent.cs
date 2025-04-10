using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonoEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerClickHandler
{
    public List<PointerEvent> pointerEventList = new List<PointerEvent>();

    UnityEvent eventEnter = new UnityEvent();
    UnityEvent eventExit = new UnityEvent();
    UnityEvent eventMove = new UnityEvent();
    UnityEvent eventDown = new UnityEvent();
    UnityEvent eventUp = new UnityEvent();
    UnityEvent eventClick = new UnityEvent();
    UnityEvent eventAwake = new UnityEvent();
    UnityEvent eventStart = new UnityEvent();
    UnityEvent eventUpdate = new UnityEvent();
    UnityEvent eventEnable = new UnityEvent();
    UnityEvent eventDisable = new UnityEvent();
    UnityEvent eventDestroy = new UnityEvent();

    private void Awake()
    {
        ResetListener();
        eventAwake?.Invoke();
    }

    void ResetListener()
    {
        foreach (var pointerEvent in pointerEventList)
        {
            switch (pointerEvent.type)
            {
                case Enum_PointerEventType.None:
                    break;
                case Enum_PointerEventType.Enter:
                    eventEnter.RemoveAllListeners();
                    eventEnter.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Exit:
                    eventExit.RemoveAllListeners();
                    eventExit.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Move:
                    eventMove.RemoveAllListeners();
                    eventMove.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Down:
                    eventDown.RemoveAllListeners();
                    eventDown.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Up:
                    eventUp.RemoveAllListeners();
                    eventUp.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Click:
                    eventClick.RemoveAllListeners();
                    eventClick.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Awake:
                    eventAwake.RemoveAllListeners();
                    eventAwake.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Start:
                    eventStart.RemoveAllListeners();
                    eventStart.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Update:
                    eventUpdate.RemoveAllListeners();
                    eventUpdate.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Enable:
                    eventEnable.RemoveAllListeners();
                    eventEnable.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Disable:
                    eventDisable.RemoveAllListeners();
                    eventDisable.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                case Enum_PointerEventType.Destroy:
                    eventDestroy.RemoveAllListeners();
                    eventDestroy.AddListener(() => { pointerEvent.eventData?.Invoke(); });
                    break;
                default:
                    break;
            }
        }
    }

    private void Update()
    {
        eventUpdate?.Invoke();
    }

    private void OnEnable()
    {
        eventEnable?.Invoke();
    }

    private void OnDisable()
    {
        eventDisable?.Invoke();
    }

    private void OnDestroy()
    {
        eventDestroy?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        eventClick?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        eventDown?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        eventEnter?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        eventExit?.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        eventMove?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        eventUp?.Invoke();
    }

    public void AddEvent(Enum_PointerEventType type, UnityAction eventData)
    {
        foreach (var pointerEvent in pointerEventList)
        {
            if (pointerEvent.type == type)
            {
                pointerEvent.eventData.AddListener(eventData);
                ResetListener();
                return;
            }
        }
        pointerEventList.Add(new PointerEvent() { type = type, eventData = new UnityEvent() });
        AddEvent(type, eventData);
    }

    public void CoverEvent(Enum_PointerEventType type, UnityAction eventData)
    {
        foreach (var pointerEvent in pointerEventList)
        {
            if (pointerEvent.type == type)
            {
                pointerEvent.eventData = new UnityEvent();
                pointerEvent.eventData.AddListener(eventData);
                ResetListener();
                return;
            }
        }
        pointerEventList.Add(new PointerEvent() { type = type, eventData = new UnityEvent() });
        CoverEvent(type, eventData);
    }

    public void RemoveEvent(Enum_PointerEventType type, UnityAction eventData)
    {
        foreach (var pointerEvent in pointerEventList)
        {
            if (pointerEvent.type == type)
            {
                pointerEvent.eventData.RemoveListener(eventData);
                ResetListener();
                return;
            }
        }

    }

    public void RemoveAllEvent(Enum_PointerEventType type)
    {
        foreach (var pointerEvent in pointerEventList)
        {
            if (pointerEvent.type == type)
            {
                pointerEvent.eventData.RemoveAllListeners();
                ResetListener();
                return;
            }
        }
    }
}

[Serializable]
public class PointerEvent
{
    public Enum_PointerEventType type;
    public UnityEvent eventData;
}

public enum Enum_PointerEventType
{
    None,
    Enter,
    Exit,
    Move,
    Down,
    Up,
    Click,
    Awake,
    Start,
    Update,
    Enable,
    Disable,
    Destroy,
}
