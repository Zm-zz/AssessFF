using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LongProgressBar : MonoBehaviour
{
    private Slider slider;
    private Button but_Custom;
    private Coroutine cor_���ӽ���;

    public UnityAction completeAction;

    /// <summary>
    /// ���������¼���
    /// </summary>
    /// <param name="autoRecovery">�Ƿ��Զ�����</param>
    public void ReRun(float time = 2, UnityAction completeAction = null, string customContent = "�鿴��", UnityAction buttonClick = null, bool autoRecovery = false)
    {
        if (!gameObject.activeInHierarchy) return;
        if (cor_���ӽ��� != null) StopCoroutine(cor_���ӽ���);
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
    /// ����������¼�
    /// </summary>
    private void ProgressComplete()
    {
        completeAction?.Invoke();
    }

    /// <summary>
    /// ����������
    /// </summary>
    private void ProgressIncrease(float fillTime, UnityAction completeEvent = null, bool autoRecovery = false)
    {
        cor_���ӽ��� = StartCoroutine(FillProgressBar(fillTime, completeEvent, autoRecovery));
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
