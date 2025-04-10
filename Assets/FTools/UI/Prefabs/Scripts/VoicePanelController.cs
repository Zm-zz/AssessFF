using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoicePanelController : MonoBehaviour
{
    public Slider slider;
    public Image img;
    public Sprite sprite_”–“Ù;
    public Sprite sprite_æ≤“Ù;

    private void Awake()
    {
        slider.onValueChanged.AddListener((f) =>
        {
            if(f == 0)
            {
                img.sprite = sprite_æ≤“Ù;
            }
            else
            {
                img.sprite = sprite_”–“Ù;
            }
            float ff = Mathf.Lerp(0.3f, 1, f);
            img.color = FMethod.SetColor_A(img.color, ff);
        });
    }
}
