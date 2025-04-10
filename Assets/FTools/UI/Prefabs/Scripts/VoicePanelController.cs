using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoicePanelController : MonoBehaviour
{
    public Slider slider;
    public Image img;
    public Sprite sprite_����;
    public Sprite sprite_����;

    private void Awake()
    {
        slider.onValueChanged.AddListener((f) =>
        {
            if(f == 0)
            {
                img.sprite = sprite_����;
            }
            else
            {
                img.sprite = sprite_����;
            }
            float ff = Mathf.Lerp(0.3f, 1, f);
            img.color = FMethod.SetColor_A(img.color, ff);
        });
    }
}
