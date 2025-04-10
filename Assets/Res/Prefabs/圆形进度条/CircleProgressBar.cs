using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CircleProgressBar : MonoBehaviour
{
    private Image progressBar;
    private Coroutine cor;

    /// <summary>
    /// 进度条重新加载
    /// </summary>
    public void ProgressReRun(float time = 2, UnityAction completeEvent = null)
    {
        progressBar = GetComponent<Image>();

        ProgressInit();
        ProgressIncrease(time, completeEvent);
    }

    /// <summary>
    /// 进度条增加
    /// </summary>
    private void ProgressIncrease(float fillTime, UnityAction completeEvent = null)
    {
        if (cor != null) StopCoroutine(cor);
        cor = StartCoroutine(FillProgressBar(fillTime, completeEvent));
    }

    private IEnumerator FillProgressBar(float fillTime, UnityAction completeEvent = null)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fillTime)
        {
            float proportion = (Time.time - startTime) / fillTime;

            progressBar.fillAmount = proportion;

            yield return null;
        }

        progressBar.fillAmount = 1;
        progressBar.transform.GetChild(0).gameObject.SetActive(true);
        progressBar.transform.GetChild(1).gameObject.SetActive(false);

        completeEvent?.Invoke();
    }

    public void ProgressInit()
    {
        progressBar.fillAmount = 0;
        progressBar.transform.GetChild(1).gameObject.SetActive(true);
        progressBar.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        if (cor != null) StopCoroutine(cor);
    }
}
