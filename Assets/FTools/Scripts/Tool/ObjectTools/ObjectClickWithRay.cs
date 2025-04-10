using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum InteractionType
{
    None,
    ClickDown,
    ClickUp,
    Enter,
    Exit
}

public class ObjectClickWithRay : MonoBehaviour
{
    public bool enable;
    public LayerMask layer;

    [Header("复原参数")]
    [HideInInspector] public Transform trans_Parent;
    [HideInInspector] public Vector3 originalPos;
    [HideInInspector] public Quaternion originalRot;

    [Header("是否渐变")]
    public bool needFade;
    [HideInInspector] public float originalMaterialAlpha;

    public UnityEvent OnMouseDown;
    public UnityEvent OnMouseUp;
    public UnityEvent OnMouseEnter;
    public UnityEvent OnMouseExit;

    // 不变
    public UnityEvent AlwaysOnMouseDown;
    public UnityEvent AlwaysOnMouseUp;
    public UnityEvent AlwaysOnMouseEnter;
    public UnityEvent AlwaysOnMouseExit;
    public bool isIn { get; private set; }

    protected virtual void Awake()
    {
        OnMouseDown.AddListener(() => { Down(); });
        OnMouseUp.AddListener(() => { Up(); });
        OnMouseEnter.AddListener(() => { Enter(); });
        OnMouseExit.AddListener(() => { Exit(); });


        AlwaysOnMouseDown.AddListener(() => { AlwaysDown(); });
        AlwaysOnMouseUp.AddListener(() => { AlwaysUp(); });
        AlwaysOnMouseEnter.AddListener(() => { AlwaysEnter(); });
        AlwaysOnMouseExit.AddListener(() => { AlwaysExit(); });

        trans_Parent = transform.parent;
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;

        if (needFade)
        {
            originalMaterialAlpha = GetComponent<Renderer>().material.color.a;
        }
    }

    protected virtual void Update()
    {
        if (!enable)
        {
            isIn = false;
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hitting = Physics.Raycast(ray, out hit, 9999, layer);

        if (hitting)
        {

            if (hit.collider.gameObject == this.gameObject)
            {
                if (isIn == false)
                {
                    isIn = true;
                    AlwaysOnMouseEnter?.Invoke();
                    OnMouseEnter?.Invoke();
                }
                if (Input.GetMouseButtonDown(0))
                {
                    AlwaysOnMouseDown?.Invoke();
                    OnMouseDown?.Invoke();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    AlwaysOnMouseUp?.Invoke();
                    OnMouseUp?.Invoke();
                }
            }
            else
            {
                if (isIn)
                {
                    isIn = false;
                    AlwaysOnMouseExit?.Invoke();
                    OnMouseExit?.Invoke();
                }
                return;
            }

        }
        else
        {
            if (isIn)
            {
                isIn = false;
                AlwaysOnMouseExit?.Invoke();
                OnMouseExit?.Invoke();
            }
        }
    }

    protected virtual void AlwaysEnter()
    {
        //移入事件
    }

    protected virtual void AlwaysExit()
    {
        //移出事件
    }

    protected virtual void AlwaysDown()
    {
        //按下事件
    }

    protected virtual void AlwaysUp()
    {
        //抬起事件
    }



    protected virtual void Enter()
    {
        //移入事件
    }

    protected virtual void Exit()
    {
        //移出事件
    }

    protected virtual void Down()
    {
        //按下事件
    }

    protected virtual void Up()
    {
        //抬起事件
    }
}
