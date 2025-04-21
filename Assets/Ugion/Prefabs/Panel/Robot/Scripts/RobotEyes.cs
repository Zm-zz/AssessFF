using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotEyes : MonoBehaviour
{
    public float a;
    public float b;
    public float speed;

    private Animation anim;
    private float timer;
    public Transform eyePos;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        timer = Random.Range(12f, 15f);
        a = (a / 1920) * Screen.width;
        b = (b / 1080) * Screen.height;
    }

    private void FixedUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        float x = eyePos.position.x - transform.position.x;
        float y = eyePos.position.y - transform.position.y;
        if (((eyePos.position.x - mousePos.x) * (eyePos.position.x - mousePos.x)) / (a * a) + ((eyePos.position.y - mousePos.y) * (eyePos.position.y - mousePos.y)) / (b * b) <= 1)
        {
            transform.position = mousePos;
            return;
        }
        transform.position += (mousePos - transform.position).normalized * speed;
        if ((x * x) / (a * a) + (y * y) / (b * b) > 1)
        {
            transform.position += (eyePos.position - transform.position).normalized * speed;
        }

        if (timer < 0)
        {
            Wink();
            timer = Random.Range(12f, 15f);
        }
        timer -= Time.deltaTime;
    }

    public void Wink()
    {
        anim.Play();
    }

    // û�в��Թ���Ŀ����Ϊ�˵���ʾ��ʾ��ʱ����������۾���λ(�������۾���ʱ���Ʈ������)
    public void OnTipShow()
    {
        transform.GetComponent<RectTransform>().anchoredPosition = eyePos.GetComponent<RectTransform>().anchoredPosition;
    }
}
