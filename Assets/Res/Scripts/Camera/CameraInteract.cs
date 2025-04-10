using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics;
using System;
using UnityEngine.Events;

public class CameraInteract : SingletonPatternMonoBase<CameraInteract>
{
    public Camera m_Camera;

    public void Start()
    {
        if (m_Camera == null)
        {
            m_Camera = Camera.main;
        }

        InitHUDFunction();
    }

    #region HUD功能
    Transform HUD;
    Transform Template;
    Dictionary<string, Transform> templateDic;
    Transform Mouse;
    void InitHUDFunction()
    {
        HUD = m_Camera.transform.Find("HUD");
        Mouse = m_Camera.transform.Find("Mouse");
        Template = HUD.Find("Template");
        templateDic = new Dictionary<string, Transform>();
        foreach (Transform t in Template)
        {
            templateDic.Add(t.name, t);
            t.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 获取预设位置
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public Transform GetTemplateTrans(string name)
    {
        if (templateDic.ContainsKey(name))
        {
            return templateDic[name];
        }
        return null;
    }

    /// <summary>
    /// 移动至预设位置
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void MoveToTemplateTrans(Transform trans, string name, float time = 0.5f, UnityAction afterAction = null)
    {
        Transform temp = GetTemplateTrans(name);
        trans.SetParent(Template);
        trans.MoveLocal(temp, time, afterAction);
    }

    /// <summary>
    /// 当前跟随鼠标的物体
    /// </summary>
    Transform mouseTarget;
    float mouseTargetZDepth;

    /// <summary>
    /// 移动至跟随鼠标位置
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="name"></param>
    /// <param name="time"></param>
    public void MoveToMouse(Transform trans, string name, float time = 0.5f)
    {
        if (mouseTarget != null)
        {
            // CDebug.Log("has exit target");
        }
        else
        {
            var t = GetTemplateTrans(name);
            if (t != null)
            {
                mouseTargetZDepth = HUD.localPosition.z + t.localPosition.z;
            }
            else
            {
                mouseTargetZDepth = HUD.localPosition.z;
            }

            mouseTarget = trans;
            StartCoroutine(MoveToMouse(trans, time));
            //trans.SetParent(Mouse);
            //trans.DOLocalMove(Vector3.zero, time);
        }
    }

    public void MoveToMouseAlginRotation(Transform trans, string name, float time = 0.5f, Vector3 bias = default)
    {
        if (mouseTarget != null)
        {
            //Debug.Log("has exit target");
        }
        else
        {
            mouseTarget = trans;

            var t = GetTemplateTrans(name);
            if (t != null)
            {
                mouseTargetZDepth = HUD.localPosition.z + t.localPosition.z;
                StartCoroutine(MoveToMouseAlignRotation(trans, t, time, bias));
            }
            else
            {
                mouseTargetZDepth = HUD.localPosition.z;
                StartCoroutine(MoveToMouse(trans, time));
            }
        }
    }

    public void ClearMouseTarget()
    {
        if (mouseTarget != null)
        {
            DOTween.Kill(mouseTarget);
            mouseTarget.SetParent(null);
            mouseTarget = null;
        }
    }

    public void ClearIfMouseTargetIsThis(Transform trans)
    {
        if (mouseTarget == trans)
        {
            DOTween.Kill(mouseTarget);
            mouseTarget.SetParent(null);
            mouseTarget = null;
        }
    }

    public void ReMoveToMouseTarget(Transform trans, float time = 1f)
    {
        if (mouseTarget != trans)
        {
            // CDebug.Log($"target error:{mouseTarget?.name},{trans?.name}");
        }
        else
        {
            StartCoroutine(MoveToMouse(mouseTarget, time));
            //mouseTarget.SetParent(Mouse);
            //mouseTarget.DOLocalMove(Vector3.zero, time);
        }
    }

    //private void StartCoroutine(IEnumerator enumerator)
    //{
    //    throw new NotImplementedException();
    //}

    private void Update()
    {
        if (mouseTarget)
        {
            SetMouseFollow();
        }
    }

    /// <summary>
    /// Mouse跟随鼠标
    /// </summary>
    void SetMouseFollow()
    {
        Vector3 m = Input.mousePosition;
        m.z = mouseTargetZDepth;
        Mouse.position = m_Camera.ScreenToWorldPoint(m);
    }

    IEnumerator MoveToMouse(Transform trans, float time)
    {
        // 刚设置父项马上进行DoLocalMove有点问题?
        // 不是这个问题，是因为Mouse进行了瞬移
        DOTween.Kill(trans);
        // 先矫正Mouse的位置，否则会瞬移
        SetMouseFollow();
        trans.SetParent(Mouse, true);
        yield return null;
        trans.DOLocalMove(Vector3.zero, time);
    }

    IEnumerator MoveToMouseAlignRotation(Transform trans, Transform to, float time, Vector3 bias)
    {
        DOTween.Kill(trans);
        SetMouseFollow();
        trans.SetParent(Mouse);
        yield return null;
        trans.DOLocalMove(bias, time);
        trans.DOLocalRotate(to.localEulerAngles, time);
        trans.DOScale(to.localScale, time);
    }
    #endregion

    #region 射线检测
    public bool GetRayHit(out RaycastHit hit)
    {
        return Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit);
    }

    /// <summary>
    /// 鼠标转世界坐标
    /// </summary>
    /// <param name="zDistance"></param>
    /// <returns></returns>
    public Vector3 GetMousePositionInWorld(float zDistance = 1f)
    {
        var mouse = Input.mousePosition;
        mouse.z = zDistance;
        return m_Camera.ScreenToWorldPoint(mouse);
    }
    #endregion

}
