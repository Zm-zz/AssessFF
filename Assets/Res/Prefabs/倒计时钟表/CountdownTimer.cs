using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText; // ������ʾ����ʱ��Text���
    public Image countDownImage;

    private float countTime;
    private float countdownTimer = 5.0f; // ����ʱʱ�䣬��λ��

    private Coroutine cor_����ʱ;

    private UnityAction completeEvent;

    public void ReCountDown(float countTime, UnityAction completeEvent = null)
    {
        if (cor_����ʱ != null) StopCoroutine(cor_����ʱ);

        this.completeEvent = completeEvent;
        this.countTime = countTime;
        this.countdownTimer = countTime;
        cor_����ʱ = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        int milliseconds = 0;
        while (countdownTimer > 0)
        {
            // ����ʣ��ĺ�����
            milliseconds = (int)(countdownTimer * 1000) % 1000;

            // ����UI��ʾ��ȷ��������ʼ����ʾ��λ����
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}",
                Mathf.FloorToInt(countdownTimer),
                Mathf.FloorToInt(countdownTimer * 1000) / 100 % 60,
                milliseconds < 10 ? "0" + milliseconds.ToString() : milliseconds.ToString());

            // �ȴ���һ֡
            yield return new WaitForSeconds(0.01f);

            // ���ٵ���ʱʱ��
            countdownTimer -= 0.01f;
            countDownImage.fillAmount = ((countTime * 1000) - countdownTimer * 1000) / (countTime * 1000);
        }

        completeEvent?.Invoke();

        // ����ʱ����
        countdownText.text = "00:00:00";
    }
}