using LipSync;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ScriptExecutionOrder(0)]
public class TalkWithMod : SingletonPatternMonoBase<TalkWithMod>
{
    public AudioLipSync sync;
    public SkinnedMeshRenderer skinOrig;

    private void Awake()
    {
        AudioManager.Instance.Stop();
        sync.audioSource = AudioManager.Instance.gameObject.transform.Find("VoiceController").GetComponent<AudioSource>();
        ResetSkin();
    }
    public void Play(AudioClip clip)
    {
        AudioManager.Instance.PlaySpecial("Tip", clip);
    }
    public void Stop()
    {
        ResetSkin();
        AudioManager.Instance.StopSpecial("Tip");
    }
    public void ResetSkin()
    {
        sync.targetBlendShapeObject = skinOrig;
    }
    public void SetSkin(SkinnedMeshRenderer skin)
    {
        sync.targetBlendShapeObject = skin;
    }

}
