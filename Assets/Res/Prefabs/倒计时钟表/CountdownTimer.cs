using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text countdownText; // 用于显示倒计时的Text组件
    public Image countDownImage;

    private float countTime;
    private float countdownTimer = 5.0f; // 倒计时时间，单位秒

    private Coroutine cor_倒计时;

    private UnityAction completeEvent;

    public void ReCountDown(float countTime, UnityAction completeEvent = null)
    {
        if (cor_倒计时 != null) StopCoroutine(cor_倒计时);

        this.completeEvent = completeEvent;
        this.countTime = countTime;
        this.countdownTimer = countTime;
        cor_倒计时 = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        int milliseconds = 0;
        while (countdownTimer > 0)
        {
            // 计算剩余的毫秒数
            milliseconds = (int)(countdownTimer * 1000) % 1000;

            // 更新UI显示，确保毫秒数始终显示两位数字
            countdownText.text = string.Format("{0:00}:{1:00}:{2:00}",
                Mathf.FloorToInt(countdownTimer),
                Mathf.FloorToInt(countdownTimer * 1000) / 100 % 60,
                milliseconds < 10 ? "0" + milliseconds.ToString() : milliseconds.ToString());

            // 等待下一帧
            yield return new WaitForSeconds(0.01f);

            // 减少倒计时时间
            countdownTimer -= 0.01f;
            countDownImage.fillAmount = ((countTime * 1000) - countdownTimer * 1000) / (countTime * 1000);
        }

        completeEvent?.Invoke();

        // 倒计时结束
        countdownText.text = "00:00:00";
    }
}