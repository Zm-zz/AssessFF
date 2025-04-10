using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public static class ExtentionUtility
{
    public static void Move(this Transform trans, Transform target, float time, UnityAction afterAction = null)
    {
        trans.DOMove(target.position, time).OnComplete(() => afterAction?.Invoke());
        trans.DORotate(target.eulerAngles, time, RotateMode.Fast);
        trans.DOScale(target.localScale, time);
    }
    public static void MoveLocal(this Transform trans, Transform target, float time, UnityAction afterAction = null)
    {
        trans.DOLocalMove(target.localPosition, time).OnComplete(() => afterAction?.Invoke());
        trans.DOLocalRotate(target.localEulerAngles, time, RotateMode.Fast);
        trans.DOScale(target.localScale, time);
    }

    //public static void Move(this Transform trans, Transform target, float time, UnityAction afterAction = null)
    //{
    //    trans.DOMove(target.position, time).OnComplete(() => afterAction?.Invoke());
    //    trans.DORotate(target.rotation.eulerAngles, time, RotateMode.Fast);
    //    trans.DOScale(target.localScale, time);
    //}
    //public static void MoveLocal(this Transform trans, Transform target, float time, UnityAction afterAction = null)
    //{
    //    trans.DOLocalMove(target.position, time).OnComplete(() => afterAction?.Invoke());
    //    trans.DOLocalRotate(target.rotation.eulerAngles, time, RotateMode.Fast);
    //    trans.DOScale(target.localScale, time);
    //}

    public static void DestoryChild(this Transform trans)
    {
        for (int i = trans.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(trans.GetChild(i).gameObject);
        }
    }

    public static int HierarchyDepth(this Transform trans)
    {
        if (trans.parent == null)
        {
            return 0;
        }
        else
        {
            return trans.parent.HierarchyDepth() + 1;
        }
    }

    public static void SetTransInit(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
    //public static TransformData GetLocalTrans(this Transform transform)
    //{
    //    return new TransformData()
    //    {
    //        position = transform.localPosition,
    //        rotation = transform.localRotation,
    //        scale = transform.localScale
    //    };
    //}
    //public static TransformData GetGlobalTrans(this Transform transform)
    //{
    //    return new TransformData()
    //    {
    //        position = transform.position,
    //        rotation = transform.rotation,
    //        scale = transform.lossyScale
    //    };
    //}
    //public static void SetLocalTrans(this Transform transform, TransformData trans)
    //{
    //    transform.localPosition = trans.position;
    //    transform.localRotation = trans.rotation;
    //    transform.localScale = trans.scale;
    //}
    //public static void SetGlobalTrans(this Transform transform, TransformData trans)
    //{
    //    transform.position = trans.position;
    //    transform.rotation = trans.rotation;
    //    transform.localScale = trans.scale;
    //}
    public static void SetTrans(this Transform transform, Transform trans)
    {
        transform.position = trans.position;
        transform.rotation = trans.rotation;
        transform.localScale = trans.localScale;
    }

    public static void DrawCircle(this LineRenderer lineRender, Vector3 position, Vector3 dir, float raduis, int lineCount)
    {
        lineRender.positionCount = lineCount;
        // 构造一个不共线的向量
        Vector3 otherDir = dir;
        float min = Mathf.Min(Mathf.Abs(dir.x), Mathf.Abs(dir.y), Mathf.Abs(dir.z));
        if (Mathf.Abs(dir.x) == min)
        {
            otherDir.x = 1f;
        }
        else if (Mathf.Abs(dir.y) == min)
        {
            otherDir.y = 1f;
        }
        else
        {
            otherDir.z = 1f;
        }

        // 构造一对正交向量
        Vector3 bias1 = Vector3.Cross(dir, otherDir).normalized;
        Vector3 bias2 = Vector3.Cross(dir, bias1).normalized;

        for (int i = 0; i < lineCount; i++)
        {
            float angle = (float)(i) / lineCount * Mathf.PI * 2;
            Vector3 r = bias1 * Mathf.Sin(angle) + bias2 * Mathf.Cos(angle);
            lineRender.SetPosition(i, position + r * raduis);
        }
    }

    public static void DrawLine(this LineRenderer lineRender, params Vector3[] points)
    {
        lineRender.positionCount = points.Length;
        lineRender.SetPositions(points);
    }

    public static T ToEnum<T>(this string str)
    {
        return (T)System.Enum.Parse(typeof(T), str);
    }

    public static void SetBlend(this Animator am, float blend)
    {
        am.SetFloat("Blend", blend);
    }
}
