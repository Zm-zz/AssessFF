using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipManager : MonoBehaviour
{
    static BoxPop TipBox;

    private void Awake()
    {
        TipBox = GetComponent<BoxPop>();
    }

    public static float ShowTips(string s, string name = "")
    {
        if (string.IsNullOrWhiteSpace(name)) name = s;
        TipBox.Show(s);
        AudioClip clip = Resources.Load<AudioClip>("Audios/" + name);
        if (clip == null)
        {
            Debug.Log($"Œ¥’“µΩ“Ù∆µ£∫{name}");
            TipBox.Show($"Œ¥’“µΩ“Ù∆µ£∫{name}");
            return 2;
        }
        else
        {
            TalkWithMod.Instance.Play(clip);
            return clip.length;
        }
    }

    public static void HideTips()
    {
        TipBox.Hide();
        TalkWithMod.Instance.Stop();
    }
}
