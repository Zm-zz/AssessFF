using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片大小不同
/// </summary>
public class PictureExplanationUpgrade : MonoBehaviour
{
    public Image[] pictures;

    public Button preBtn;
    public Button nextBtn;
    public Button closeBtn;
    public GameObject panelSelf;

    int index;

    private void Awake()
    {
        preBtn.onClick.AddListener(OnClick_preBtn);
        nextBtn.onClick.AddListener(OnClick_nextBtn);
        closeBtn.onClick.AddListener(OnClick_closeBtn);
    }

    void SetActive(bool b)
    {
        panelSelf.SetActive(b);
    }

    public void SetKnowledge()
    {
        SetKnowledge(0);
        SetActive(true);
    }

    void SetKnowledge(int index)
    {
        this.index = index;
        foreach(var img in pictures)
        {
            img.gameObject.SetActive(false);
        }
        pictures[index].gameObject.SetActive(true);
        preBtn.gameObject.SetActive(true);
        nextBtn.gameObject.SetActive(true);
        if (index == 0) preBtn.gameObject.SetActive(false);
        if (index == pictures.Length - 1) nextBtn.gameObject.SetActive(false);
    }
    void OnClick_preBtn()
    {
        SetKnowledge(index - 1);
    }
    void OnClick_nextBtn()
    {
        SetKnowledge(index + 1);
    }
    void OnClick_closeBtn()
    {
        SetActive(false);
    }
}
