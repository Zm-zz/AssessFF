using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogBoxController : MonoBehaviour
{
    BoxPop dialogBox;
    public GameObject icon;
    public UnityAction<AudioClip> audioPlay = null;
    public UnityAction audioStop = null;

    private void Awake()
    {
        dialogBox = GetComponentInChildren<BoxPop>();
        HideDialog();
        if (icon.GetComponent<Image>().sprite == null) icon.SetActive(false);
    }

    public void SetAction(UnityAction<AudioClip> audioPlay, UnityAction audioStop)
    {
        this.audioPlay = audioPlay;
        this.audioStop = audioStop;
    }
    public float PlayDialog(string info, string audioName = "")
    {
        dialogBox.Hide();
        if (audioName == "") audioName = info;
        AudioClip clip = Resources.Load<AudioClip>("Audios/" + audioName);
        if (clip != null)
        {
            audioPlay?.Invoke(clip);
            dialogBox.Show(info, clip.length);
        }
        else
        {
            dialogBox.Show(info, 2);
            Debug.LogWarning("“Ù∆µ»± ß£∫" + info);
        }
        return clip == null ? 3 : clip.length + 0.5f;
    }
    public void HideDialog()
    {
        dialogBox.Hide();
        audioStop?.Invoke();
    }

    public void HideDialogImmediately()
    {
        dialogBox.HideImmediately();
        audioStop?.Invoke();
    }

    private void Update()
    {
        if (icon.GetComponent<Image>().sprite != null) icon.SetActive(dialogBox.transform.localScale.x > 0);
    }
}
