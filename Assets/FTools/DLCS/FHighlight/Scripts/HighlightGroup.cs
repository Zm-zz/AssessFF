using HighlightPlus;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightGroup : MonoBehaviour
{
    public HighlightEffect[] hilights;

    public void Hilighted(bool b)
    {
        foreach (var h in hilights)
        {
            h.highlighted = b;
        }
    }
}
