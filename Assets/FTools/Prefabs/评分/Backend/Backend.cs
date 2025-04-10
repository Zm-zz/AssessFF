using System;
using UnityEngine;
using UnityEngine.Networking;

public class Backend : MonoBehaviour
{
    static Backend _instance;
    public static Backend Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("backend");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<Backend>();
            }
            return _instance;
        }
    }

    public UserInfo UserInfo;

    BackendData _backendData;

    bool _TrainBegin;   // 确保开始和提交是同一个模式

    public void Init(BackendData data)
    {
        _backendData = data;

#if UNITY_EDITOR
        data.IsStandalone = true;
#endif
        string appProtocol = _backendData.AppProtocol + "://";

        if (!data.IsStandalone)
        {
            try
            {

#if UNITY_WEBGL
                UlpsBaseReqUtil.init(_backendData.AppID, appProtocol, "/ulps/",
                    _backendData.AppVersion);
#elif UNITY_STANDALONE
                UlpsBaseReqUtil.init(_backendData.AppID, appProtocol,
                    _backendData.AppVersion);
#endif
            }
            catch
            {

            }
        }
        else
        {
            try
            {
                UlpsBaseReqUtil.init(_backendData.AppID, appProtocol, "http://47.110.151.175:8517/ulps/",
                    _backendData.AppVersion);
                UlpsBaseReqUtil.login("lzstudent", "asdasd", (a) => { Debug.Log("测试登录成功"); }, (a) => { Debug.Log("测试登录失败"); }, this);
            }
            catch
            {

            }
        }
    }

    public void BeginTrain(bool IsTrain)
    {
        Action<UnityWebRequest> whenSuccess = request =>
        {
            Debug.Log("成功开始训练，响应为：\n" + request.downloadHandler.text);
            GetUserInfo();
        };
        try
        {
            if (IsTrain)
            {
                UlpsBaseReqUtil.startTrain(whenSuccess, whenErrorMyDo, this);
            }
            else
            {
                UlpsBaseReqUtil.startExam(whenSuccess, whenErrorMyDo, this);
            }

            _TrainBegin = IsTrain;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void Submit(ResultVo result)
    {
        Action<UnityWebRequest> whenSuccess = request =>
        {
            Debug.Log("提交成绩成功，响应为：\n" + request.downloadHandler.text);
        };
        try
        {
            if (_TrainBegin)
            {
                UlpsBaseReqUtil.submitTrain(result, whenSuccess, whenErrorDoNothing, this);
            }
            else
            {
                UlpsBaseReqUtil.submitExam(result, whenSuccess, whenErrorDoNothing, this);
            }
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    void GetUserInfo()
    {
        Action<UserInfo> whenSuccess = (info) => 
        {
            UserInfo = info;
        };
        UlpsBaseReqUtil.getUserInfo(whenSuccess, whenErrorDoNothing, this);
    }

#region error response

    void whenErrorMyDo(UnityWebRequest webRequest)
    {
        //根据不同的状态码给出不同的反馈
        string errMsg;
        switch (webRequest.responseCode)
        {
            case 401: errMsg = "请先登录"; break;
            case 404: errMsg = "网络异常"; break;
            case 400: errMsg = webRequest.downloadHandler.text; break;
            default: errMsg = "系统错误"; break;
        }

        if (!_backendData.IsStandalone)
        {
            QuitAPP();
        }

        Debug.Log(errMsg);
    }

    void whenErrorDoNothing(UnityWebRequest webRequest)
    {
        string errMsg;
        switch (webRequest.responseCode)
        {
            case 401: errMsg = "请先登录"; break;
            case 404: errMsg = "网络异常"; break;
            case 400: errMsg = webRequest.downloadHandler.text; break;
            default: errMsg = "系统错误"; break;
        }

        Debug.Log(errMsg);
    }

    void QuitAPP()
    {
//#if !UNITY_EDITOR
//        Application.Quit();
//#endif
    }

#endregion
}

[Serializable]
public struct BackendData
{
    public bool IsStandalone;
    public string AppID;        // "S-000131"
    public string AppProtocol;  // "UGION000131"
    public string AppVersion;   // "1.0"
}     