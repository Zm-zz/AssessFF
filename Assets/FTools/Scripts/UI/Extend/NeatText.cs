using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NeatText : MonoBehaviour
{
    /// <summary>
    /// Text�ı����
    /// </summary>
    private Text txt;

    /// <summary>
    /// ����ƥ������ţ�������ʽ��
    /// </summary>
    private readonly string strRegex = @"(\��|\��|\��|\��|\��|\��|\��|\��|\��|\��|\��|\��|\+|\-)";

    /// <summary>
    /// ���ڴ洢text����е�����
    /// </summary>
    private System.Text.StringBuilder MExplainText = null;

    /// <summary>
    /// ���ڴ洢text�������е�����
    /// </summary>
    private IList<UILineInfo> MExpalinTextLine;

    private void Awake()
    {
        txt = GetComponent<Text>();

        OnTextChange();
        txt.RegisterDirtyLayoutCallback(OnTextChange);
    }

    private void OnTextChange()
    {
        StartCoroutine(MClearUpExplainMode(txt, txt.text));
    }

    /// <summary>
    /// �������֡�ȷ������ĸ�����ֱ��
    /// </summary>
    /// <param name="_component">text���</param>
    /// <param name="_text">��Ҫ����text�е�����</param>
    /// <returns></returns>
    IEnumerator MClearUpExplainMode(Text _component, string _text)
    {
        _component.text = _text;

        //���ֱ��ִ���±߷����Ļ�����ô_component.cachedTextGenerator.lines�����ȡ����֮ǰtext�е����ݣ�������_text�����ݣ�������Ҫ�ȴ�һ��
        yield return new WaitForEndOfFrame();

        MExpalinTextLine = _component.cachedTextGenerator.lines;

        //��Ҫ�ı���ַ����
        int mChangeIndex = -1;

        MExplainText = new System.Text.StringBuilder(_component.text);

        for (int i = 1; i < MExpalinTextLine.Count; i++)
        {
            if (_component.text.Length <= MExpalinTextLine[i].startCharIdx)
                break;

            //��λ�Ƿ��б��
            bool _b = Regex.IsMatch(_component.text[MExpalinTextLine[i].startCharIdx].ToString(), strRegex);

            if (_b)
            {
                // mChangeIndex = MExpalinTextLine[i].startCharIdx - 1;
                // mChangeIndex = _GetInsertPos(_component, MExpalinTextLine[i].startCharIdx - 1);
                mChangeIndex = GetInsertPos(_component, MExpalinTextLine[i].startCharIdx - 1, MExpalinTextLine[i - 1].startCharIdx);
                if (mChangeIndex > 0)
                    MExplainText.Insert(mChangeIndex, "\n");
            }
        }

        _component.text = MExplainText.ToString();
    }

    private int GetInsertPos(Text _component, int startCharIdx, int lastLineStartIdx)
    {
        bool _b = Regex.IsMatch(_component.text[startCharIdx].ToString(), strRegex);
        if (_b)
        {
            startCharIdx = _GetInsertPos(_component, startCharIdx - 1);
            if (startCharIdx <= lastLineStartIdx)
                startCharIdx = 0;
        }

        return startCharIdx;
    }

    private int _GetInsertPos(Text _component, int startCharIdx)
    {
        if (startCharIdx == 0)
            return 0;
        bool _b = Regex.IsMatch(_component.text[startCharIdx].ToString(), strRegex);
        if (_b)
        {
            return _GetInsertPos(_component, startCharIdx - 1);
        }

        return startCharIdx;
    }
}