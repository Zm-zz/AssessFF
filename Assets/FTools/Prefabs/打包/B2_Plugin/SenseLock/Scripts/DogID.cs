using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SenseLock;
using System;
using System.IO;


public class DogID : MonoBehaviour
{
    private HardwareLock _lock = null;
    [SerializeField]
    private uint _licenseID;
    [SerializeField]
    private float _timeInterval = 30f;

    private float _time = 0;
    // Use this for initialization
    void Start()
    {
        SetPath();
        _lock = new HardwareLock();
        if (_lock.CheckState(_licenseID))
        {

        }
        else
        {
            Application.Quit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if(_time > _timeInterval)
        {
            _time = 0;
            if (_lock.CheckState(_licenseID))
            {

            }
            else
            {
                Application.Quit();
            }
        }
    }

    private void SetPath() // static Constructor
    {
        var currentPath = Environment.GetEnvironmentVariable("PATH",
            EnvironmentVariableTarget.Process);
#if UNITY_EDITOR_32
    var dllPath = Application.dataPath
        + Path.DirectorySeparatorChar + "SenseLock"
        + Path.DirectorySeparatorChar + "Plugins";
        // + Path.DirectorySeparatorChar + "x86";
#elif UNITY_EDITOR_64
        var dllPath = Application.dataPath
            + Path.DirectorySeparatorChar + "SenseLock"
            + Path.DirectorySeparatorChar + "Plugins";
        // + Path.DirectorySeparatorChar + "x86_64";
#else // Player
    var dllPath = Application.dataPath
        + Path.DirectorySeparatorChar + "Plugins";

#endif
        if (currentPath != null && currentPath.Contains(dllPath) == false)
            Environment.SetEnvironmentVariable("PATH", currentPath + Path.PathSeparator
                + dllPath, EnvironmentVariableTarget.Process);
    }
}
