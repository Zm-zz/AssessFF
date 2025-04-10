using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasFade : MonoBehaviour
{
    Coroutine cor;
    public CanvasGroup canvasGroup;
    public float intervalTime;

    public void Show()
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(Appear());
    }
    public void ShowImmediately()
    {
        canvasGroup.alpha = 1;
    }

    public void Hide()
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(Disappear());
    }
    public void HideImmediately()
    {
        canvasGroup.alpha = 0;
    }

    IEnumerator Appear()
    {
        float alpha = canvasGroup.alpha;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * (1.0f/intervalTime);
            canvasGroup.alpha = alpha;
            yield return new WaitForFixedUpdate();
        }
        alpha = 1;
        canvasGroup.alpha = alpha;
    }

    IEnumerator Disappear()
    {
        float alpha = canvasGroup.alpha;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * (1.0f / intervalTime);
            canvasGroup.alpha = alpha;
            yield return new WaitForFixedUpdate();
        }
        alpha = 0;
        canvasGroup.alpha = alpha;
    }
}
