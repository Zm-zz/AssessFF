using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitterRefresh : MonoBehaviour
{
    private void LateUpdate()
    {
        foreach (var rect in GetComponentsInChildren<RectTransform>())
        {
            try
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
            }
            catch
            {
                Debug.LogWarning("UIË¢ÐÂ´íÎó");
            }
        }
    }
}