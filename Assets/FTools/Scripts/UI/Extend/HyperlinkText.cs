using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// �ı��ؼ�,֧�ֳ�����,<a href= index>����</a>
/// </summary>
public class HyperlinkText : Text, IPointerClickHandler
{
    /// <summary>
    /// ��������Ϣ��
    /// </summary>
    private class HyperlinkInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }

    /// <summary>
    /// ���������յ��ı�
    /// </summary>
    private string m_OutputText;

    /// <summary>
    /// ��������Ϣ�б�
    /// </summary>
    private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

    /// <summary>
    /// �ı�������
    /// </summary>
    protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

    [Serializable]
    public class HrefClickEvent : UnityEvent<string> { }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// �����ӵ���¼�
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }


    /// <summary>
    /// ����������
    /// </summary>
    private static readonly Regex s_HrefRegex = new Regex(@"<a href=([^>\n\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    private HyperlinkText mHyperlinkText;


    public string GetHyperlinkInfo
    {
        get { return text; }
    }

    protected override void Awake()
    {
        base.Awake();
        mHyperlinkText = GetComponent<HyperlinkText>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        mHyperlinkText.onHrefClick.AddListener(OnHyperlinkTextInfo);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        mHyperlinkText.onHrefClick.RemoveListener(OnHyperlinkTextInfo);
    }


    public override void SetVerticesDirty()
    {
        base.SetVerticesDirty();
#if UNITY_EDITOR
#pragma warning disable CS0618 // ���ͻ��Ա�ѹ�ʱ
        if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
#pragma warning restore CS0618 // ���ͻ��Ա�ѹ�ʱ
        {
            return;
        }
#endif
        text = GetHyperlinkInfo;
        m_OutputText = GetOutputText(text);

    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        var orignText = m_Text;
        m_Text = m_OutputText;
        base.OnPopulateMesh(toFill);
        m_Text = orignText;
        UIVertex vert = new UIVertex();

        _mins.Clear();
        _sizes.Clear();
        // �������Ӱ�Χ��
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // ��������������ı���������������뵽��Χ��
            toFill.PopulateUIVertex(ref vert, hrefInfo.startIndex);
            var bounds = new Bounds(vert.position, Vector3.zero);

            for (int i = hrefInfo.startIndex; i < hrefInfo.endIndex; i += 4)
            {
                if (i + 3 >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                var pos0 = vert.position;
                toFill.PopulateUIVertex(ref vert, i + 1);
                var pos1 = vert.position;
                toFill.PopulateUIVertex(ref vert, i + 2);
                var pos2 = vert.position;
                toFill.PopulateUIVertex(ref vert, i + 3);
                var pos3 = vert.position;

                if (pos2.x < bounds.max.x) // ����������Ӱ�Χ��
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    _mins.Add(bounds.min);
                    _sizes.Add(bounds.size);

                    bounds = new Bounds(pos0, Vector3.zero);
                    bounds.Encapsulate(pos1); // ��չ��Χ��
                    bounds.Encapsulate(pos2); // ��չ��Χ��
                    bounds.Encapsulate(pos3); // ��չ��Χ��
                }
                else
                {
                    bounds.Encapsulate(pos0); // ��չ��Χ��
                    bounds.Encapsulate(pos1); // ��չ��Χ��
                    bounds.Encapsulate(pos2); // ��չ��Χ��
                    bounds.Encapsulate(pos3); // ��չ��Χ��
                }
            }

            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));

            _mins.Add(bounds.min);
            _sizes.Add(bounds.size);
            //Debug.Log(bounds.min);
            //Debug.Log(bounds.size);
        }
    }

    readonly List<Vector2> _mins = new List<Vector2>();
    readonly List<Vector2> _sizes = new List<Vector2>();

    void OnDrawGizmos()
    {
        Vector2 position = transform.position;
        for (int i = 0; i < _mins.Count; i++)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(_mins[i] + position + _sizes[i] / 2, _sizes[i]);
        }
    }

    /// <summary>
    /// ��ȡ�����ӽ�������������ı�
    /// </summary>
    /// <returns></returns>
    Regex _regex = new Regex("<.*?>");
    protected virtual string GetOutputText(string outputText)
    {
        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear();
        var indexText = 0;
        foreach (Match match in s_HrefRegex.Matches(outputText))
        {
            s_TextBuilder.Append(outputText.Substring(indexText, match.Index - indexText));
            s_TextBuilder.Append("<color=blue>");  // ��������ɫ

            string textBuilderWithOutRich = _regex.Replace(s_TextBuilder.ToString(), "");
            textBuilderWithOutRich = textBuilderWithOutRich.Replace("\r", "");

            var group = match.Groups[1];
            var hrefInfo = new HyperlinkInfo
            {
                startIndex = textBuilderWithOutRich.Length * 4, // ����������ı���ʼ��������
                endIndex = (textBuilderWithOutRich.Length + match.Groups[2].Length - 1) * 4 + 3,
                name = group.Value
            };
            m_HrefInfos.Add(hrefInfo);

            s_TextBuilder.Append(match.Groups[2].Value);
            s_TextBuilder.Append("</color>");
            indexText = match.Index + match.Length;
        }
        s_TextBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        return s_TextBuilder.ToString();
    }

    /// <summary>
    /// ����¼�����Ƿ������������ı�
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Vector2 lp = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, eventData.position, eventData.pressEventCamera, out lp);

        foreach (var hrefInfo in m_HrefInfos)
        {
            var boxes = hrefInfo.boxes;
            for (var i = 0; i < boxes.Count; ++i)
            {
                if (boxes[i].Contains(lp))
                {
                    m_OnHrefClick.Invoke(hrefInfo.name);
                    return;
                }
            }
        }
    }
    /// <summary>
    /// ��ǰ��������ӻص�
    /// </summary>
    /// <param name="info">�ص���Ϣ</param>
    private void OnHyperlinkTextInfo(string info)
    {
        Debug.Log(info);
        if (int.TryParse(info, out int result))
        {
            //if(result < actions.Count)actions[result]?.Invoke();
        }
    }

    public List<Action> actions = new List<Action>();
}