using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashImage : MonoBehaviour
{
    public Image img;

    public float maxAlpha;
    public float minAlpha;
    public float delta;

    bool go = true;
    float timer;

    private void OnEnable()
    {
        timer = delta;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = delta;
            go = !go;
            if (go)
            {
                img.CrossFadeAlpha(maxAlpha, delta, false);
            }
            else
            {
                img.CrossFadeAlpha(minAlpha, delta, false);
            }
        }
    }
}
