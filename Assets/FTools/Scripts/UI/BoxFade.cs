using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxFade : MonoBehaviour
{
    public static BoxFade Instance;  //全局静态调用，可以不用

    List<RectTransform> rects = new List<RectTransform>();
    Image img;
    Text text;
    float alpha;
    Coroutine cor;
    CanvasGroup canvasGroup;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        foreach(RectTransform rect in GetComponentsInChildren<RectTransform>())
        {
            rects.Add(rect);
        }
        img = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        if(!gameObject.TryGetComponent<CanvasGroup>(out canvasGroup))
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = 0;
    }

    public void Hide()
    {
        if (cor != null) StopCoroutine(nameof(Disappear));
        canvasGroup.alpha = 0;
    }

    public void PopLog(string s, float time = 3f)
    {
        if (cor != null) StopCoroutine(nameof(Disappear));
        cor = StartCoroutine(nameof(Disappear), time);
        text.text = s;
        foreach (var i in rects)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(i);
        }
        alpha = 1;
        canvasGroup.alpha = 1;
    }

    public void PopLogAlways(string s)
    {
        text.text = s;
        foreach (var i in rects)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(i);
        }
        alpha = 1;
        canvasGroup.alpha = 1;
    }

    IEnumerator Disappear(float time)
    {
        yield return new WaitForSeconds(time);
        while (alpha > 0)
        {
            alpha -= Time.deltaTime;
            canvasGroup.alpha = alpha;
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
