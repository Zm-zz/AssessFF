using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wink : MonoBehaviour
{
    Animation anim;
    public bool doWink;
    [SerializeField] private AnimationClip wink;
    public Vector2 winkInterval = new Vector2(0,0);
    float timer;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        AnimationState state = anim[wink.name];
        state.layer = 1;
    }
    private void Update()
    {
        if (doWink)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                anim.Play(wink.name);
                timer = UnityEngine.Random.Range(winkInterval.x, winkInterval.y);
            }
        }
    }
}
