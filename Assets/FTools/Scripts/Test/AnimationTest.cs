using CustomInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationTest : MonoBehaviour
{
    Animation anim;
    public int animIndex;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }

    [InspectorButton("²¥¶¯»­")]
    void PlayAnim()
    {
        anim.Stop();
        int i = 0;
        foreach(AnimationState state in anim)
        {
            if(i == animIndex)
            {
                anim.clip = state.clip;
                anim.Play();
                return;
            }
            i++;
        }
    }
}
