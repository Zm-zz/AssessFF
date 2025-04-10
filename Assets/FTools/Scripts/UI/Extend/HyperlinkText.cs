using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 文本控件,支持超链接,<a href= index>描述</a>
/// </summary>
public class HyperlinkText : Text, IPointerClickHandler
{
    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HyperlinkInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect>();
    }

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string m_OutputText;

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    protected static readonly StringBuilder s_TextBuilder = new StringBuilder();

    [Serializable]
    public class HrefClickEvent : UnityEvent<string> { }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }


    /// <summary>
    /// 超链接正则
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
#pragma warning disable CS0618 // 类型或成员已过时
        if (UnityEditor.PrefabUtility.GetPrefabType(this) == UnityEditor.PrefabType.Prefab)
#pragma warning restore CS0618 // 类型或成员已过时
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
        // 处理超链接包围框
        foreach (var hrefInfo in m_HrefInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.startIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
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

                if (pos2.x < bounds.max.x) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    _mins.Add(bounds.min);
                    _sizes.Add(bounds.size);

                    bounds = new Bounds(pos0, Vector3.zero);
                    bounds.Encapsulate(pos1); // 扩展包围框
                    bounds.Encapsulate(pos2); // 扩展包围框
                    bounds.Encapsulate(pos3); // 扩展包围框
                }
                else
                {
                    bounds.Encapsulate(pos0); // 扩展包围框
                    bounds.Encapsulate(pos1); // 扩展包围框
                    bounds.Encapsulate(pos2); // 扩展包围框
                    bounds.Encapsulate(pos3); // 扩展包围框
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
    /// 获取超链接解析后的最后输出文本
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
            s_TextBuilder.Append("<color=blue>");  // 超链接颜色

            string textBuilderWithOutRich = _regex.Replace(s_TextBuilder.ToString(), "");
            textBuilderWithOutRich = textBuilderWithOutRich.Replace("\r", "");

            var group = match.Groups[1];
            var hrefInfo = new HyperlinkInfo
            {
                startIndex = textBuilderWithOutRich.Length * 4, // 超链接里的文本起始顶点索引
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
    /// 点击事件检测是否点击到超链接文本
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
    /// 当前点击超链接回调
    /// </summary>
    /// <param name="info">回调信息</param>
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