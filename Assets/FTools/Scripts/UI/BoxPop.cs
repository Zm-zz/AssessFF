using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// µ¯³ö¶Ô»°¿ò
/// </summary>
public class BoxPop : MonoBehaviour
{
    float popSpeed = 5f;
    public Text popInfoText;
    Coroutine popCor;
    Coroutine hideCor;
    bool isShown;

    public void SetString(string str)
    {
        popInfoText.text = str;
    }

    public void Show()
    {
        if (popCor != null) StopCoroutine(popCor);
        popCor = StartCoroutine(nameof(Popping));
        if (hideCor != null) StopCoroutine(hideCor);
    }
    public void Show(string str)
    {
        SetString(str);
        Show();
    }
    public void Show(float time)
    {
        Show();
        hideCor = StartCoroutine(nameof(InvokeHide), time);
    }
    public void Show(string str, float time)
    {
        SetString(str);
        Show(time);
    }

    public void SetBox(Vector3 pos, string str = "")
    {
        transform.position = pos;
        if (str != "")
        {
            SetString(str);
        }
        Show();
    }

    public void SetShow(bool b)
    {
        if (b)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
    public void ShowSwitch()
    {
        if (isShown)
        {
            Hide();
        }
        else
        {
            Show();
        }
        isShown = !isShown;
    }

    public void Hide()
    {
        if (popCor != null) StopCoroutine(popCor);
        popCor = StartCoroutine(nameof(Hidding));
    }

    public void HideImmediately()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (hideCor != null) StopCoroutine(hideCor);
        transform.localScale = Vector3.zero;
        isShown = false;
    }

    public void ShowImmediately()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (hideCor != null) StopCoroutine(hideCor);
        transform.localScale = Vector3.one;
        isShown = true;
    }

    IEnumerator Popping()
    {
        while (transform.localScale.x < 1)
        {
            transform.localScale += new Vector3(popSpeed, popSpeed, popSpeed) * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.one;
        isShown = true;
        yield break;
    }

    IEnumerator Hidding()
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(popSpeed, popSpeed, popSpeed) * Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
        isShown = false;
        yield break;
    }

    IEnumerator InvokeHide(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }
}
