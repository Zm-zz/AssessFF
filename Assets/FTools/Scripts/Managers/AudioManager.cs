using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
/// <summary>
/// 音频管理器
/// </summary>
public class AudioManager : SingletonPatternMonoAutoBase_DontDestroyOnLoad<AudioManager>
{
    //各个声道的AudioSource组件
    AudioSource bgmAudioSource;
    AudioSource bgsAudioSource;
    AudioSource voiceAudioSource;

    Dictionary<string, AudioSource> specialDict;
    List<AudioSource> sound2DList;
    List<AudioSource> sound3DList;
    List<AudioSource> msList;

    //各个声道的游戏对象
    GameObject bgmController;
    GameObject soundController;
    GameObject sound2D;
    GameObject sound3D;
    GameObject bgsController;
    GameObject msController;
    GameObject voiceController;
    GameObject specialController;

    //控制器的名字
    string bgmControllerName = "BgmController";
    string soundControllerName = "SoundController";
    string soundPool2DName = "SoundPool2D";
    string soundClip2DName = "SoundClip2D";
    string soundPool3DName = "SoundPool3D";
    string soundClip3DName = "SoundClip3D";
    string bgsControllerName = "BgsController";
    string msControllerName = "MsController";
    string msClipName = "MsClip";
    string voiceControllerName = "VoiceController";
    string specialControllerName = "specialController";

    //各个声道的声音音量
    float totalVolume = 1;
    float bgmVolume = 1;
    float sound2DVolume = 1;
    float sound3DVolume = 1;
    float bgsVolume = 1;
    float msVolume = 1;
    float voiceVolume = 1;

    //各个声道的声音速度
    float bgmSpeed = 1;
    float sound2DSpeed = 1;
    float sound3DSpeed = 1;
    float bgsSpeed = 1;
    float msSpeed = 1;
    float voiceSpeed = 1;

    #region BGM控制
    /// <summary>
    /// 播放BGM
    /// </summary>
    public void PlayBGM(AudioClip bgm, bool loop = true)
    {
        if (bgm == null)
        {
            Debug.LogWarning("播放BGM失败！要播放的BGM为null");
            return;
        }

        bgmAudioSource.loop = loop;
        bgmAudioSource.clip = bgm;
        bgmAudioSource.Play();
    }

    /// <summary>
    /// 暂停BGM
    /// </summary>
    public void PauseBGM()
    {
        bgmAudioSource.Pause();
    }

    /// <summary>
    /// 取消暂停BGM
    /// </summary>
    public void UnPauseBGM()
    {
        bgmAudioSource.UnPause();
    }

    /// <summary>
    /// 停止BGM
    /// </summary>
    public void StopBGM()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }

    /// <summary>
    /// 设置背景音音量
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        bgmAudioSource.volume = bgmVolume;
    }

    /// <summary>
    /// 设置背景音音量
    /// </summary>
    public void SetBGMSpeed(float speed)
    {
        bgmSpeed = Mathf.Clamp(speed, -10, 10);
        bgmAudioSource.pitch = bgmSpeed;
    }
    #endregion

    #region 2D音效控制
    /// <summary>
    /// 播放2D音效
    /// </summary>
    public void PlaySound(AudioClip sound)
    {
        if (sound == null)
        {
            Debug.LogWarning("播放Sound失败！要播放的Sound为null");
            return;
        }

        //临时的空物体，用来播放音效。
        GameObject go = null;

        for (int i = 0; i < sound2D.transform.childCount; i++)
        {
            //如果对象池中有，则从对象池中取出来用。
            if (!sound2D.transform.GetChild(i).gameObject.activeSelf)
            {
                go = sound2D.transform.GetChild(i).gameObject;
                go.SetActive(true);
                break;
            }
        }
        //如果对象池中没有，则创建一个游戏对象。
        if (go == null)
        {
            go = new GameObject(soundClip2DName);
            go.transform.SetParent(sound2D.transform);
        }

        //如果该游戏对象身上没有AudioSource组件，则添加AudioSource组件并设置参数。
        if (!go.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            sound2DList.Add(audioSource);
        }

        //设置要播放的音效
        audioSource.clip = sound;

        //设置音量
        audioSource.volume = sound2DVolume;

        //设置速度
        audioSource.pitch = sound2DSpeed;

        //播放音效
        audioSource.Play();

        //每隔1秒检测一次，如果该音效播放完毕，则销毁音效的游戏对象。
        StartCoroutine(DestroyWhenFinished());

        //每隔1秒检测一次，如果该音效播放完毕，则销毁音效的游戏对象。
        IEnumerator DestroyWhenFinished()
        {
            do
            {
                yield return new WaitForSeconds(1);

                if (go == null || audioSource == null) yield break;//如果播放音频的游戏对象，或者AudioSource组件被销毁了，则直接跳出协程。
            } while (audioSource != null && audioSource.time > 0);

            if (go != null)
            {
                audioSource.clip = null;
                go.SetActive(false);
            }

        }
    }

    /// <summary>
    /// 暂停2D音效
    /// </summary>
    public void Pause2DSound()
    {
        foreach (var source in sound2DList)
        {
            source.Pause();
        }
    }

    /// <summary>
    /// 取消暂停2D音效
    /// </summary>
    public void UnPause2DSound()
    {
        foreach (var source in sound2DList)
        {
            source.UnPause();
        }
    }

    /// <summary>
    /// 停止2D音效
    /// </summary>
    public void Stop2DSound()
    {
        foreach (var source in sound2DList)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置2D音效音量
    /// </summary>
    public void SetSound2DVolume(float volume)
    {
        sound2DVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        foreach (var source in sound2DList)
        {
            source.volume = sound2DVolume;
        }
    }

    /// <summary>
    /// 设置2D音效速度
    /// </summary>
    public void SetSound2DSpeed(float speed)
    {
        sound2DSpeed = Mathf.Clamp(speed, -10, 10);
        foreach (var source in sound2DList)
        {
            source.pitch = sound2DSpeed;
        }
    }
    #endregion

    #region 3D音效控制
    /// <summary>
    /// 播放3D音效
    /// </summary>
    public void PlaySound(AudioClip sound, GameObject target)
    {
        if (sound == null)
        {
            Debug.LogWarning("播放Sound失败！要播放的Sound为null");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("播放Sound失败！无法在目标对象身上播放Sound，因为目标对象为null");
            return;
        }

        //临时的空物体，用来播放音效。
        GameObject go = null;

        for (int i = 0; i < sound3D.transform.childCount; i++)
        {
            //如果对象池中有，则从对象池中取出来用。
            if (!sound3D.transform.GetChild(i).gameObject.activeSelf)
            {
                go = sound3D.transform.GetChild(i).gameObject;
                go.SetActive(true);
                break;
            }
        }
        //如果对象池中没有，则创建一个游戏对象。
        if (go == null)
        {
            go = new GameObject(soundClip3DName);
        }

        //把用于播放音效的游戏对象放到目标物体之下，作为它的子物体
        go.transform.SetParent(target.transform);
        go.transform.localPosition = Vector3.zero;

        //如果该游戏对象身上没有AudioSource组件，则添加AudioSource组件并设置参数。
        if (!go.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f;//3D效果，近大远小。
            audioSource.dopplerLevel = 0;
        }

        //设置要播放的音频
        audioSource.clip = sound;

        //设置音量
        audioSource.volume = sound3DVolume;

        //设置速度
        audioSource.pitch = sound3DSpeed;

        //播放音频
        audioSource.Play();

        //每隔1秒检测一次，如果该音频播放完毕，则销毁游戏对象。
        StartCoroutine(DestoryWhenFinisied());

        //每隔1秒检测一次，如果该音频播放完毕，则销毁游戏对象。
        IEnumerator DestoryWhenFinisied()
        {
            do
            {
                yield return new WaitForSeconds(1);

                if (go == null || audioSource == null) yield break;//如果播放音频的游戏对象，或者AudioSource组件被销毁了，则直接跳出协程。
            } while (audioSource != null && audioSource.time > 0);

            if (go != null)
            {
                //放入对象池
                go.transform.SetParent(sound3D.transform);
                go.transform.localPosition = Vector3.zero;
                audioSource.clip = null;
                go.SetActive(false);
            }

        }
    }

    /// <summary>
    /// 在世界空间中指定的位置播放3D音效
    /// </summary>
    public void PlaySound(AudioClip sound, Vector3 worldPosition, Transform parent = null)
    {
        if (sound == null)
        {
            Debug.LogWarning("播放Sound失败！要播放的Sound为null");
            return;
        }

        //临时的空物体，用来播放音效。
        GameObject go = null;

        for (int i = 0; i < sound3D.transform.childCount; i++)
        {
            //如果对象池中有，则从对象池中取出来用。
            if (!sound3D.transform.GetChild(i).gameObject.activeSelf)
            {
                go = sound3D.transform.GetChild(i).gameObject;
                go.SetActive(true);
                break;
            }
        }
        //如果对象池没有，则创建一个游戏对象。
        if (go == null)
        {
            go = new GameObject(soundClip3DName);
        }

        go.transform.position = worldPosition;
        go.transform.SetParent(parent);


        //如果该游戏对象身上没有AudioSource组件，则添加AudioSource组件并设置参数。
        if (!go.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            audioSource.spatialBlend = 1f;//3D效果，近大远小。
            sound3DList.Add(audioSource);
        }

        //设置要播放的音频
        audioSource.clip = sound;

        //设置音量
        audioSource.volume = sound3DVolume;

        //播放音频
        audioSource.Play();

        //每隔一秒检测一次，如果该音频播放完毕，则销毁该游戏对象。
        StartCoroutine(DestroyWhenFinished());

        IEnumerator DestroyWhenFinished()
        {
            do
            {
                yield return new WaitForSeconds(1);

                if (go == null || audioSource == null) yield break;//如果播放音频的游戏对象，或者AudioSource组件被销毁了，则直接跳出协程。

            } while (audioSource != null && audioSource.time > 0);

            if (go != null)
            {
                //放入对象池
                go.transform.SetParent(sound3D.transform);
                go.transform.localPosition = Vector3.zero;
                audioSource.clip = null;
                go.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 暂停3D音效
    /// </summary>
    public void Pause3DSound()
    {
        foreach (var source in sound3DList)
        {
            source.Pause();
        }
    }

    /// <summary>
    /// 取消暂停3D音效
    /// </summary>
    public void UnPause3DSound()
    {
        foreach (var source in sound3DList)
        {
            source.UnPause();
        }
    }

    /// <summary>
    /// 停止3D音效
    /// </summary>
    public void Stop3DSound()
    {
        foreach (var source in sound3DList)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置3D音效音量
    /// </summary>
    public void SetSound3DVolume(float volume)
    {
        sound3DVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        foreach (var source in sound3D.GetComponentsInChildren<AudioSource>())
        {
            source.volume = sound3DVolume;
        }
    }

    ///<summary>
    /// 设置3D音效速度
    /// </summary>
    public void SetSound3DSpeed(float speed)
    {
        sound3DSpeed = Mathf.Clamp(speed, -10, 10);
        foreach (var source in sound3D.GetComponentsInChildren<AudioSource>())
        {
            source.pitch = sound3DSpeed;
        }
    }
    #endregion

    #region 环境音控制
    /// <summary>
    /// 播放环境音效
    /// </summary>
    public void PlayBGS(AudioClip bgs, bool loop = true)
    {
        if (bgs == null)
        {
            Debug.LogWarning("播放BGS失败！要播放的BGS为null");
            return;
        }

        bgsAudioSource.loop = loop;
        bgsAudioSource.clip = bgs;
        bgsAudioSource.Play();
    }

    /// <summary>
    /// 暂停环境音效
    /// </summary>
    public void PauseBGS()
    {
        bgsAudioSource.Pause();
    }

    /// <summary>
    /// 取消暂停环境音效
    /// </summary>
    public void UnPauseBGS()
    {
        bgsAudioSource.UnPause();
    }

    /// <summary>
    /// 停止BGS
    /// </summary>
    public void StopBGS()
    {
        bgsAudioSource.Stop();
        bgsAudioSource.clip = null;
    }

    /// <summary>
    /// 设置环境音音量
    /// </summary>
    public void SetBGSVolume(float volume)
    {
        bgsVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        bgsAudioSource.volume = bgsVolume;
    }

    /// <summary>
    /// 设置环境音速度
    /// </summary>
    public void SetBGSSpeed(float speed)
    {
        bgsSpeed = Mathf.Clamp(speed, -10, 10);
        bgsAudioSource.pitch = bgsSpeed;
    }
    #endregion

    #region 提示音控制
    /// <summary>
    /// 播放提示音效
    /// </summary>
    /// <param name="ms"></param>
    public void PlayMS(AudioClip ms)
    {
        if (ms == null)
        {
            Debug.LogWarning("播放MS失败！要播放的MS为null");
            return;
        }

        //临时的空物体，用来播放提示音效
        GameObject go = null;

        for (int i = 0; i < msController.transform.childCount; i++)
        {
            //如果对象池中有，则从对象池中取出来用。
            if (!msController.transform.GetChild(i).gameObject.activeSelf)
            {
                go = msController.transform.GetChild(i).gameObject;
                go.SetActive(true);
                break;
            }
        }
        //如果对象池中没有，则创建一个游戏对象。
        if (go == null)
        {
            go = new GameObject(msClipName);
            go.transform.SetParent(msController.transform);
        }

        //如果该游戏对象身上有AudioSource组件，则添加AudioSource组件并设置参数。
        if (!go.TryGetComponent<AudioSource>(out AudioSource audioSource))
        {
            audioSource = go.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = false;
            msList.Add(audioSource);
        }

        //设置要播放的音频
        audioSource.clip = ms;

        //设置音量
        audioSource.volume = msVolume;

        //设置速度
        audioSource.pitch = msSpeed;

        //播放提示音效
        audioSource.Play();
        msList.Add(audioSource);

        //每隔一秒检测一次，如果该提示音效播放完毕，则销毁。
        StartCoroutine(DestroyWhenFinished());

        //每隔一秒检测一次，如果该提示音效播放完毕，则销毁。
        IEnumerator DestroyWhenFinished()
        {
            do
            {
                yield return new WaitForSeconds(1);

                if (go == null || audioSource == null) yield break;//如果播放音频的游戏对象，或者AudioSource组件被销毁了，则直接跳出协程。

            } while (audioSource != null && audioSource.time > 0);

            if (go != null)
            {
                //放入对象池
                go.transform.SetParent(msController.transform);
                audioSource.clip = null;
                go.SetActive(false);
                if (msList.Contains(audioSource)) msList.Remove(audioSource);
            }
        }
    }

    /// <summary>
    /// 暂停提示音效
    /// </summary>
    public void PauseMSSound()
    {
        foreach (var source in msList)
        {
            source.Pause();
        }
    }

    /// <summary>
    /// 取消暂停提示音效
    /// </summary>
    public void UnPauseMSSound()
    {
        foreach (var source in msList)
        {
            source.UnPause();
        }
    }

    /// <summary>
    /// 停止提示音效
    /// </summary>
    public void StopMSSound()
    {
        foreach (var source in msList)
        {
            source.Stop();
            source.clip = null;
            source.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 设置提示音效音量
    /// </summary>
    public void SetMSVolume(float volume)
    {
        msVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        foreach (var source in msList)
        {
            source.volume = msVolume;
        }
    }

    /// <summary>
    /// 设置提示音速度
    /// </summary>
    public void SetMSSpeed(float speed)
    {
        msSpeed = Mathf.Clamp(speed, -10, 10);
        foreach (var source in msList)
        {
            source.pitch = msSpeed;
        }
    }
    #endregion

    #region 唯一角色音控制
    /// <summary>
    /// 播放唯一角色语音
    /// </summary>
    public void PlayVoice(AudioClip voice)
    {
        voiceAudioSource.clip = voice;
        voiceAudioSource.Play();
    }

    /// <summary>
    /// 暂停唯一角色语音
    /// </summary>
    public void PauseVoice()
    {
        voiceAudioSource.Pause();
    }

    /// <summary>
    /// 取消暂停唯一角色语音
    /// </summary>
    public void UnPauseVoice()
    {
        voiceAudioSource.UnPause();
    }

    /// <summary>
    /// 停止唯一角色语音
    /// </summary>
    public void StopVoice()
    {
        voiceAudioSource.Stop();
        voiceAudioSource.clip = null;
    }

    /// <summary>
    /// 设置角色音音量
    /// </summary>
    public void SetVoiceVolume(float volume)
    {
        voiceVolume = Mathf.Clamp(volume * totalVolume, 0, 1);
        voiceAudioSource.volume = voiceVolume;
    }

    /// <summary>
    /// 设置角色音速度
    /// </summary>
    public void SetVoiceSpeed(float speed)
    {
        voiceSpeed = Mathf.Clamp(speed, -10, 10);
        voiceAudioSource.pitch = voiceSpeed;
    }
    #endregion

    #region 特殊音频控制
    /// <summary>
    /// 播放特殊音频
    /// </summary>
    public void PlaySpecial(string name, AudioClip clip, bool loop = false, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogWarning("播放Sound失败！无法在目标对象身上播放Sound，因为目标对象为null");
            return;
        }
        AudioSource audioSource;
        if (!specialDict.TryGetValue(name, out audioSource))
        {
            GameObject go;
            go = new GameObject(name);
            go.transform.SetParent(specialController.transform);
            audioSource = go.AddComponent<AudioSource>();
            specialDict.Add(name, audioSource);
        }
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 0f;//3D效果，近大远小。
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

    /// <summary>
    /// 在3D物体上播放特殊音频
    /// </summary>
    public void PlaySpecial(string name, AudioClip clip, GameObject target, bool loop = false, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogWarning("播放Sound失败！无法在目标对象身上播放Sound，因为目标对象为null");
            return;
        }
        if (target == null)
        {
            Debug.LogWarning("播放Sound失败！无法在目标对象身上播放Sound，因为目标对象为null");
            return;
        }
        GameObject go;
        AudioSource audioSource;
        if (!specialDict.TryGetValue(name, out audioSource))
        {
            go = new GameObject(name);
            go.transform.SetParent(specialController.transform);
            audioSource = go.AddComponent<AudioSource>();
            specialDict.Add(name, audioSource);
        }
        else
        {
            go = audioSource.gameObject;
        }
        go.transform.SetParent(target.transform);
        go.transform.localPosition = Vector3.zero;
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;//3D效果，近大远小。
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

    /// <summary>
    /// 在空间中指定位置播放特殊音频
    /// </summary>
    public void PlaySpecial(string name, AudioClip clip, Vector3 worldPosition, Transform parent = null, bool loop = false, float volume = 1)
    {
        if (clip == null)
        {
            Debug.LogWarning("播放Sound失败！无法在目标对象身上播放Sound，因为目标对象为null");
            return;
        }
        GameObject go;
        AudioSource audioSource;
        if (!specialDict.TryGetValue(name, out audioSource))
        {
            go = new GameObject(name);
            go.transform.SetParent(specialController.transform);
            audioSource = go.AddComponent<AudioSource>();
            specialDict.Add(name, audioSource);
        }
        else
        {
            go = audioSource.gameObject;
        }
        go.transform.position = worldPosition;
        go.transform.SetParent(parent);
        audioSource.loop = loop;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f;//3D效果，近大远小。
        audioSource.dopplerLevel = 0;
        audioSource.Play();
    }

    /// <summary>
    /// 暂停某个特殊音频
    /// </summary>
    public void PauseSpecial(string name)
    {
        AudioSource audioSource;
        if (specialDict.TryGetValue(name, out audioSource))
        {
            audioSource.Pause();
        }
        else
        {
            Debug.LogWarning($"未找到对应音频{name}");
        }
    }

    /// <summary>
    /// 取消暂停某个特殊音频
    /// </summary>
    /// <param name="name"></param>
    public void UnPauseSpecial(string name)
    {
        AudioSource audioSource;
        if (specialDict.TryGetValue(name, out audioSource))
        {
            audioSource.UnPause();
        }
        else
        {
            Debug.LogWarning($"未找到对应音频{name}");
        }
    }

    /// <summary>
    /// 暂停所有特殊音频
    /// </summary>
    public void PauseSpecial()
    {
        foreach (var audioSource in specialDict.Values)
        {
            audioSource.Pause();
        }
    }

    /// <summary>
    /// 取消暂停所有特殊音频
    /// </summary>
    public void UnPauseSpecial()
    {
        foreach (var audioSource in specialDict.Values)
        {
            audioSource.UnPause();
        }
    }

    /// <summary>
    /// 停止某个特殊音频
    /// </summary>
    public void StopSpecial(string name)
    {
        AudioSource audioSource;
        if (specialDict.TryGetValue(name, out audioSource))
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
        else
        {
            Debug.LogWarning($"未找到对应音频{name}");
        }
    }

    /// <summary>
    /// 停止所有特殊音频
    /// </summary>
    public void StopSpecial()
    {
        foreach (var audioSource in specialDict.Values)
        {
            audioSource.Stop();
            audioSource.clip = null;
        }
    }

    /// <summary>
    /// 设置某个特殊音频音量
    /// </summary>
    public void SetSpecialVolume(string name, float volume)
    {
        AudioSource audioSource;
        if (specialDict.TryGetValue(name, out audioSource))
        {
            audioSource.volume = Mathf.Clamp(volume * totalVolume, 0, 1);
        }
        else
        {
            Debug.LogWarning($"未找到对应音频{name}");
        }
    }

    /// <summary>
    /// 设置整体特殊音频音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetSpecialVolume(float volume)
    {
        foreach (var audioSource in specialDict.Values)
        {
            audioSource.volume = Mathf.Clamp(volume * totalVolume, 0, 1);
        }
    }

    /// <summary>
    /// 获取特殊音频的AudioSource
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public AudioSource GetSpecialAudioSource(string name)
    {
        AudioSource audioSource;
        if (specialDict.TryGetValue(name, out audioSource))
        {
            return audioSource;
        }
        else
        {
            Debug.LogWarning($"未找到对应音频{name}");
            return null;
        }
    }

    /// <summary>
    /// 设置整体特殊音频速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpecialSpeed(float speed)
    {
        foreach (var audioSource in specialDict.Values)
        {
            audioSource.pitch = Mathf.Clamp(speed, -10, 10);
        }
    }
    #endregion

    #region 整体控制
    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void Stop()
    {
        StopBGM();
        StopBGS();
        Stop2DSound();
        Stop3DSound();
        StopMSSound();
        StopVoice();
        StopSpecial();
    }

    /// <summary>
    /// 暂停所有音效
    /// </summary>
    public void Pause()
    {
        PauseBGM();
        PauseBGS();
        Pause2DSound();
        Pause3DSound();
        PauseMSSound();
        PauseVoice();
        PauseSpecial();
    }

    /// <summary>
    /// 取消暂停所有音效
    /// </summary>
    public void UnPause()
    {
        UnPauseBGM();
        UnPauseBGS();
        UnPause2DSound();
        UnPause3DSound();
        UnPauseMSSound();
        UnPauseVoice();
        UnPauseSpecial();
    }

    /// <summary>
    /// 设置所有音效速度
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(float speed)
    {
        SetBGMSpeed(speed);
        SetBGSSpeed(speed);
        SetSound2DSpeed(speed);
        SetSound3DSpeed(speed);
        SetMSSpeed(speed);
        SetVoiceSpeed(speed);
        SetSpecialSpeed(speed);
    }

    /// <summary>
    /// 设置整体音量
    /// </summary>
    /// <param name="volume"></param>
    public void SetTotalVolume(float volume)
    {
        totalVolume = Mathf.Clamp(volume, 0, 1);
    }
    #endregion

    void Awake()
    {
        //创建并设置背景音乐的控制器
        bgmController = CreateController(bgmControllerName, transform);
        bgmAudioSource = bgmController.AddComponent<AudioSource>();
        bgmAudioSource.playOnAwake = false;
        bgmAudioSource.loop = true;

        //创建音效控制器
        soundController = CreateController(soundControllerName, transform);
        sound2D = CreateController(soundPool2DName, soundController.transform);
        sound3D = CreateController(soundPool3DName, soundController.transform);
        sound2DList = new List<AudioSource>();
        sound3DList = new List<AudioSource>();

        //创建并设置环境音效的控制器
        bgsController = CreateController(bgsControllerName, transform);
        bgsAudioSource = bgsController.AddComponent<AudioSource>();
        bgsAudioSource.playOnAwake = false;
        bgsAudioSource.loop = true;

        //创建提示音效的控制器
        msController = CreateController(msControllerName, transform);
        msList = new List<AudioSource>();

        //创建并设置角色语音的控制器
        voiceController = CreateController(voiceControllerName, transform);
        voiceAudioSource = voiceController.AddComponent<AudioSource>();
        voiceAudioSource.playOnAwake = false;
        voiceAudioSource.loop = false;

        specialController = CreateController(specialControllerName, transform);
        specialDict = new Dictionary<string, AudioSource>();

        EventCenterManager.AddListener(GameCommand.暂停, Pause);
        EventCenterManager.AddListener(GameCommand.取消暂停, UnPause);
        EventCenterManager.AddListener(GameCommand.加速, (float speed) => { SetSpeed(speed); });
    }
    GameObject CreateController(string name, Transform parent)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent);
        return go;
    }
}
