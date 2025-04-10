using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ����
/// </summary>
public class BoxSlider : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    enum PopType 
    {
        //�����Ƴ�
        Enter,
        //�������
        Click,
        //���ش���
        Toggle,
    }

    [Tooltip("����������")]
    public RectTransform trans;
    [Tooltip("��������(���뵯��/�������)")]
    [SerializeField] PopType type;
    [Tooltip("��ʼλ��")]
    public Vector2 startPos;
    [Tooltip("����λ��")]
    public Vector2 endPos;
    [Tooltip("�����ٶ�")]
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
    /// ����
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
    /// �ջ�
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
    /// �ص���ʼλ��
    /// </summary>
    public void ToStart()
    {
        trans.anchoredPosition = startPos;
    }

    /// <summary>
    /// �ص��յ�λ��
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

    #region ��������
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
