using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 弹框
/// </summary>
public class BoxSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    enum PopType 
    {
        //移入移出
        Enter,
        //点击弹出
        Click,
        //开关触发
        Toggle,
    }

    [Tooltip("弹出的物体")]
    public RectTransform trans;
    [Tooltip("弹出类型(移入弹出/点击弹出)")]
    [SerializeField] PopType type;
    [Tooltip("初始位置")]
    public Vector2 startPos;
    [Tooltip("弹出位置")]
    public Vector2 endPos;
    [Tooltip("弹出速度")]
    public float speed;
       
    Coroutine popCor;          
    float dis;
    Toggle tog;

    bool popping;

    private void Awake()
    {
        dis = (startPos - endPos).sqrMagnitude;
        popping = false;

        if (type == PopType.Toggle)
        {
            tog = GetComponent<Toggle>();
            tog.onValueChanged.AddListener((bool b)=> { OnValueChange(b); } );
        }
    }

    public void OnValueChange(bool able)
    {
        if (type != PopType.Toggle) return;
        if(able)
        {
            PlayPop();
        }
        else
        {
            PlayHide();
        }
    }

    /// <summary>
    /// 弹出
    /// </summary>
    public void PlayPop()
    {
        if (popCor != null)
        {
            StopCoroutine(popCor);
        }
        popCor = StartCoroutine(nameof(Pop));
        popping = true;
    }

    /// <summary>
    /// 收回
    /// </summary>
    public void PlayHide()
    {
        if (popCor != null)
        {
            StopCoroutine(popCor);
        }
        popCor = StartCoroutine(nameof(Hide));
        popping = false;
    }

    /// <summary>
    /// 回到初始位置
    /// </summary>
    public void ToStart()
    {
        trans.anchoredPosition = startPos;
    }

    /// <summary>
    /// 回到终点位置
    /// </summary>
    public void ToEnd()
    {
        trans.anchoredPosition = endPos;
    }

    #region MouseEvent
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type != PopType.Enter) return;
        PlayPop();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (type != PopType.Enter) return;
        PlayHide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (type != PopType.Click) return;
        if (popping)
        {
            PlayHide();
        }
        else
        {
            PlayPop();
        }
    }
    #endregion 

    #region 弹出动画
    IEnumerator Pop()
    {
        while ((trans.anchoredPosition - startPos).sqrMagnitude < dis)
        {
            trans.anchoredPosition = Vector3.MoveTowards(trans.anchoredPosition, endPos, speed * Time.deltaTime * 100);
            yield return new WaitForFixedUpdate();
        }
        trans.anchoredPosition = endPos;
        yield break;
    }

    IEnumerator Hide()
    {
        while ((trans.anchoredPosition - endPos).sqrMagnitude < dis)
        {
            trans.anchoredPosition = Vector3.MoveTowards(trans.anchoredPosition, startPos, speed * Time.deltaTime * 100);
            yield return new WaitForFixedUpdate();
        }
        trans.anchoredPosition = startPos;
        yield break;
    }
    #endregion
}
