using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongProgressBar : MonoBehaviour
{
    private Slider slider;
    private Button but_Custom;
    private Coroutine cor_增加进度;

    public UnityAction completeAction;

    /// <summary>
    /// 进度条重新加载
    /// </summary>
    /// <param name="autoRecovery">是否自动回收</param>
    public void ReRun(float time = 2, UnityAction completeAction = null, string customContent = "查看中", UnityAction buttonClick = null, bool autoRecovery = false)
    {
        if (!gameObject.activeInHierarchy) return;
        if (cor_增加进度 != null) StopCoroutine(cor_增加进度);
        this.completeAction = completeAction;

        slider = GetComponentInChildren<Slider>();
        but_Custom = GetComponentInChildren<Button>();

        but_Custom.GetComponent<Text>().text = customContent;
        but_Custom.onClick.RemoveAllListeners();
        if (buttonClick != null)
        {
            but_Custom.onClick.AddListener(buttonClick);
            but_Custom.onClick.AddListener(completeAction);
            if (autoRecovery)
                but_Custom.onClick.AddListener(() => ObjectPoolsManager.Instance.Despawn(gameObject));
        }

        ProgressInit();
        ProgressIncrease(time, ProgressComplete, autoRecovery);
    }

    /// <summary>
    /// 进度条完成事件
    /// </summary>
    private void ProgressComplete()
    {
        completeAction?.Invoke();
    }

    /// <summary>
    /// 进度条增加
    /// </summary>
    private void ProgressIncrease(float fillTime, UnityAction completeEvent = null, bool autoRecovery = false)
    {
        cor_增加进度 = StartCoroutine(FillProgressBar(fillTime, completeEvent, autoRecovery));
    }

    private IEnumerator FillProgressBar(float fillTime, UnityAction completeEvent = null, bool autoRecovery = false)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fillTime)
        {
            float proportion = (Time.time - startTime) / fillTime;

            slider.value = proportion;

            yield return null;
        }

        slider.value = 1;

        completeEvent?.Invoke();

        if (autoRecovery)
        {
            ObjectPoolsManager.Instance.Despawn(gameObject);
        }
    }

    private void ProgressInit()
    {
        slider.value = 0;
    }
}
