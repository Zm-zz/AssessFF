using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCameraManager : SingletonPatternMonoBase<VirtualCameraManager>
{
    GameObject curCamera;
    public GameObject initCamera;

    private void Awake()
    {
        curCamera = initCamera;
        MoveToInitial();
    }

    public void SetInitCamera(GameObject initCamera)
    {
        curCamera.SetActive(false);
        this.initCamera = initCamera;
        curCamera = initCamera;
        curCamera.SetActive(true);
    }

    public void MoveToInitial()
    {
        curCamera.SetActive(false);
        curCamera = initCamera;
        curCamera.SetActive(true);
    }

    public void MoveTowards(GameObject targetCamera)
    {
        curCamera.SetActive(false);
        curCamera = targetCamera;
        curCamera.SetActive(true);
    }
}
