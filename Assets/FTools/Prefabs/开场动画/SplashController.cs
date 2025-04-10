using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(5)]
public class SplashController : MonoBehaviour
{
    public SpriteRenderer sr;
    public Image img;
    bool isEnd = false;

    private void Awake()
    {
#if UNITY_EDITOR
        gameObject.SetActive(false);
#endif
    }

    private void Update()
    {
        if (img.sprite.name != "¿ª»ú¶¯»­_0000_Í¼²ã 149")
        {
            img.sprite = sr.sprite;
        }
        else
        {
            if (!isEnd)
            {
                isEnd = true;
            }
        }
        if (isEnd)
        {
            img.color = FMethod.SetColor_A(img.color, img.color.a - Time.deltaTime);
            if (img.color.a <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
