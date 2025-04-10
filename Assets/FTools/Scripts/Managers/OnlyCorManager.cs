using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlyCorManager : SingletonPatternMonoAutoBase<OnlyCorManager>
{
    Coroutine cor;

    public void StartCor(IEnumerator enumerator)
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(enumerator);
    }

    public void StartCor(string enumerator)
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(enumerator);
    }

    public void StopCor()
    {
        if (cor != null) StopCoroutine(cor);
    }
}
