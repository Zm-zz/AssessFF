using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Reflection;

/// <summary>
/// 护理平台-PC软件对接后台
/// </summary>
//[Obsolete]
public class UlpsBaseReqUtil
{
    //全局的基本信息
    public static string basePath;//服务器后端基础路径(打包后，由前端启动参数初始化)
    public static string trainStartUrl = "practice/train/start/";//开始训练(参数不完整，有待补充appId作为路径参数)
    public static string trainSubmitUrl = "practice/train/submit/";//提交训练成绩(参数不完整，有待补充appId作为路径参数)
    public static string examStartUrl = "practice/exam/start/";//开始考核(参数不完整，有待补充appId作为路径参数)
    public static string examSubmitUrl = "practice/exam/submit/";//提交考核成绩(参数不完整，有待补充appId作为路径参数) 
    public static string roleUrl = "user/role";//获取用户身份
    public static string studentInfoUrl = "student/self";//获取学生信息
    public static string teacherInfoUrl = "teacher/self";//获取教师信息
    public static string loginUrl = "login";//登录
    public static string logoutUrl = "logout";//登出

    public static string sessionCookie = "";//会话标识(打包后，由前端启动参数初始化)
    public static string sceneId;//本应用的id
    public static string appProtocol;//注册表所用的协议前缀(例如：protocol://)
    public static string version;//当前应用的版本

    //测试用变量
    public static string contentToShow;

    //——————————————————————————————————初始化————————————————————————————
    //init-编辑器下使用-初始化与后端交互的必要信息（包括对以下信息的初始化：应用id,应用协议,后端访问路径,会话标识）
    public static void init(string sceneId, string appProtocol, string basePath, string version)
    {
        UlpsBaseReqUtil.sceneId = sceneId;
        UlpsBaseReqUtil.basePath = basePath;
        UlpsBaseReqUtil.appProtocol = appProtocol;
        UlpsBaseReqUtil.version = version;
        //对初始化情况做检测
        checkBaseInfo();
    }
    //init-打包时使用-初始化与后端的交互信息
    public static void init(string sceneId, string appProtocol, string version)
    {
        UlpsBaseReqUtil.sceneId = sceneId;
        UlpsBaseReqUtil.appProtocol = appProtocol;
        UlpsBaseReqUtil.version = version;
        //获取命令行参数并初始化相关变量
        string[] CommandLineArgs = Environment.GetCommandLineArgs();
        if (CommandLineArgs != null && CommandLineArgs.Length > 1)
        {
            //获取实际要用的那部分启动参数
            string tempArgs = CommandLineArgs[1];//此时对应的是Protocal://args/这部分
            tempArgs= UnityWebRequest.UnEscapeURL(tempArgs, System.Text.Encoding.UTF8);
            tempArgs = tempArgs.Substring(appProtocol.Length);//此时对应的是args这部分
            //获取启动参数内容，并设置相关的变量
            JsonData param = JsonMapper.ToObject(tempArgs);
            sessionCookie = param["sessionCookie"].ToString();
            basePath = param["basePath"].ToString();
            contentToShow = basePath;
        }
        else
        {
            throw new Exception("启动参数异常");
        }
        //对初始化情况的检测
        checkBaseInfo();
    }

    private static void checkBaseInfo()
    {
        if (isEmptyStr(basePath)) throw new Exception("后端基础路径尚未初始化");
        if (isEmptyStr(appProtocol)) throw new Exception("应用协议尚未初始化");
    }

    //——————————————————————————————————后端接口调用————————————————————————————
    //登录
    public static void login(string username, string password, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + loginUrl;
        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");

        WWWForm form = new WWWForm();

        form.AddField("username", username);
        form.AddField("password", password);

        Action<UnityWebRequest> whenSuccessX = request =>
        {
            //根据响应更新sessionCookie
            string cookieStr = request.GetResponseHeader("Set-Cookie");
            if (cookieStr == null || cookieStr.Equals("")) throw new Exception("响应异常");
            string[] cookies = cookieStr.Split(';');
            if (cookies.Length < 1) throw new Exception("响应异常");
            sessionCookie = cookies[0];
            //执行用户自定义回调
            whenSuccess(request);
        };
        item.StartCoroutine(Post(url, form, whenSuccessX, whenError));
    }
    //登出
    public static void logout(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + logoutUrl;
        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //开始训练
    public static void startTrain(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + trainStartUrl + sceneId;

        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //提交训练成绩
    public static void submitTrain(ResultVo result, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + trainSubmitUrl + sceneId;
        //构建请求参数
        string resultJson = JsonMapper.ToJson(result);
        Debug.Log("构造出的json串:\n" + resultJson);
        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");
        item.StartCoroutine(Post(url, resultJson, whenSuccess, whenError));
    }
    //开始考核
    public static void startExam(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + examStartUrl + sceneId;

        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //提交考核成绩
    public static void submitExam(ResultVo result, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + examSubmitUrl + sceneId;
        //构建请求参数
        string resultJson = JsonMapper.ToJson(result);
        Debug.Log("构造出的json串:\n" + resultJson);
        if (item == null) throw new Exception("请提供可用的MonoBehavior对象，用于发送请求");
        item.StartCoroutine(Post(url, resultJson, whenSuccess, whenError));
    }
    //获取用户身份
    public static void getRole(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + roleUrl;
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //获取学生信息
    public static void getStudentInfo(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + studentInfoUrl;
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //获取教师信息
    public static void getTeacherInfo(Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        string url = basePath + teacherInfoUrl;
        item.StartCoroutine(Get(url, whenSuccess, whenError));
    }
    //获取用户信息（对教师和学生给出了统一的信息表示）
    public static void getUserInfo(Action<UserInfo> whenSuccess, Action<UnityWebRequest> whenError, MonoBehaviour item)
    {
        Action<UnityWebRequest> whenSuccessRole = (request) =>
        {
            string data = JsonMapper.ToObject<RespData<string>>(request.downloadHandler.text).data;
            switch (data)
            {
                case "STUDENT":
                    //获取学生信息并转化为用户信息
                    Action<UnityWebRequest> whenSuccessStu = (stuRequest) =>
                    {
                        StudentInfo info = RespData<object>.parseData(stuRequest.downloadHandler.text, new StudentInfo());//学生信息
                        whenSuccess(UserInfo.transFromStudent(info));
                    };
                    getStudentInfo(whenSuccessStu, whenError, item);
                    break;
                case "TEACHER":
                    //获取教师信息并转化为用户信息
                    Action<UnityWebRequest> whenSuccessTeacher = (teacherRequest) =>
                    {
                        TeacherInfo info = RespData<object>.parseData(teacherRequest.downloadHandler.text, new TeacherInfo());//学生信息
                        whenSuccess(UserInfo.transFromTeacher(info));
                    };
                    getTeacherInfo(whenSuccessTeacher, whenError, item);
                    break;
                default: break;
            }
        };
        item.StartCoroutine(Get(basePath + roleUrl, whenSuccessRole, whenError));
    }

    //————————————————————————————————请求方法——————————————————————————————————
    static IEnumerator Get(string url, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get(url);
        if (sessionCookie != null && sessionCookie != "") webRequest.SetRequestHeader("Cookie", sessionCookie);

        yield return webRequest.SendWebRequest();
        //收到响应后的处理逻辑
#pragma warning disable CS0618 // 类型或成员已过时
        if (webRequest.isHttpError || webRequest.isNetworkError)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            whenError(webRequest);
        }
        else
        {
            whenSuccess(webRequest);
        }
    }
    static IEnumerator Post(string url, JsonData data, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError)
    {
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(data.ToJson());

        UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        webRequest.uploadHandler = new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        if (sessionCookie != null && sessionCookie != "") webRequest.SetRequestHeader("Cookie", sessionCookie);

        yield return webRequest.SendWebRequest();

#pragma warning disable CS0618 // 类型或成员已过时
        if (webRequest.isHttpError || webRequest.isNetworkError)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            whenError(webRequest);
        }
        else
        {
            whenSuccess(webRequest);
        }
    }
    static IEnumerator Post(string url, string jsonParam, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError)
    {
        byte[] postBytes = System.Text.Encoding.Default.GetBytes(jsonParam);

        UnityWebRequest webRequest = new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
        webRequest.uploadHandler = new UploadHandlerRaw(postBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        if (sessionCookie != null && sessionCookie != "") webRequest.SetRequestHeader("Cookie", sessionCookie);

        yield return webRequest.SendWebRequest();

#pragma warning disable CS0618 // 类型或成员已过时
        if (webRequest.isHttpError || webRequest.isNetworkError)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            whenError(webRequest);
        }
        else
        {
            whenSuccess(webRequest);
        }
    }
    static IEnumerator Post(string url, WWWForm formData, Action<UnityWebRequest> whenSuccess, Action<UnityWebRequest> whenError)
    {
        UnityWebRequest webRequest = UnityWebRequest.Post(url, formData);
        //webRequest.downloadHandler = new DownloadHandlerBuffer();
        //webRequest.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        if (sessionCookie != null && sessionCookie != "") webRequest.SetRequestHeader("Cookie", sessionCookie);

        yield return webRequest.SendWebRequest();

#pragma warning disable CS0618 // 类型或成员已过时
        if (webRequest.isHttpError || webRequest.isNetworkError)
#pragma warning restore CS0618 // 类型或成员已过时
        {
            whenError(webRequest);
        }
        else
        {
            whenSuccess(webRequest);
        }
    }

    //————————————————————————————请求结果处理示例————————————————————————————————
    //请求成功的简单处理示例
    public static void whenSuccessNormalDo(UnityWebRequest webRequest)
    {
        Debug.Log("请求成功，响应为：\n" + webRequest.downloadHandler.text);
    }
    //请求失败的简单处理示例
    public static void whenErrorNormalDo(UnityWebRequest webRequest)
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
        Debug.LogWarning(errMsg);
    }
    //————————————————————————————————辅助方法——————————————————————————————————
    private static Boolean isEmptyStr(string str)
    {
        return str == null || str == "";
    }
}
//————————————————————————————————实体类——————————————————————————————————
//提交成绩相关
public class ResultVo//整体结果
{
    public double totalScore;//满分（不是用户得分，是当前训练的满分）
    public List<StepDetail> details;//各步骤的得分细节
    public string moduleLabel;

    public ResultVo CloneClass()
    {
        return (ResultVo)MemberwiseClone();
    }
}
public class StepDetail//某步骤相关数据
{
    public bool done;//当前步骤是否做了
    public List<PointDetail> pointDetails;//当前步骤下各得分点的情况
}
public class PointDetail//某得分点相关数据
{
    public string action;//用户行为
    public double total;//这一得分点的满分
    public double scoreGet;//用户得分

    public void GetScore() { scoreGet = total; }
}
//后端基本响应数据
public class RespData<T>
{
    public string code;//后端响应码(目前没什么用)
    public string msg;//后端错误信息(目前没什么用)
    public T data;//有意义的响应内容

    public static RespData<T> parse(string json)//data为基本类型解析用这个
    {
        return JsonMapper.ToObject<RespData<T>>(json);
    }

    public static D parseData<D>(string resp,D data) where D:Data<D>//data为复杂类型解析用这个
    {
        JsonData jsonData = JsonMapper.ToObject(resp);
        return data.fromJsonData(jsonData["data"]);
    }
}
//需要用到的响应数据实体

public interface Data<T>
{
    /// <summary>
    /// 利用data内容构建T类的实体对象
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    T fromJsonData(JsonData data);
}
public class StudentInfo:Data<StudentInfo>
{
    public int id;
    public string username;//用户名
    public string number;//学号
    public string email;//邮箱
    public string name;//姓名
    public string gender;//性别
    public int classroomId;//班级id
    public string classroomName;//班级名称
    public int departmentId;//院系id
    public string departmentName;//院系名称
    public int specialtyId;//专业id
    public string specialtyName;//专业名称
    public int gradeId;//年级id
    public string gradeName;//年级名称
    public int status;//0-禁用;1-启用
    public string school;//学校

    public StudentInfo fromJsonData(JsonData json)
    {
        FieldInfo[] fields = GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            Type type = field.FieldType;
            JsonData node = json[field.Name];
            if (node == null || "".Equals(node.ToString())) continue;
            if (type == typeof(int))
            {
                //Debug.Log(field.Name + "是整数类型,值为：" + int.Parse(node.ToString()));
                field.SetValue(this, int.Parse(node.ToString()));
            }
            else if (type == typeof(string))
            {
                //Debug.Log(field.Name + "是字符串类型，值为：" + node.ToString());
                field.SetValue(this, node.ToString());
            }
            else
            {
                throw new Exception("系统错误-反序列化时未指定[" + type + "]类型的赋值方式");
            }
        }
        return this;
    }
}

public class TeacherInfo:Data<TeacherInfo>
{
    public int id;
    public string username;//用户名
    public string number;//工号
    public string email;//邮箱
    public string name;//姓名
    public string gender;//性别
    public string school;
    public string classrooms;//相关班级列表
    public int status;//0-禁用;1-启用

    public TeacherInfo fromJsonData(JsonData json)
    {
        FieldInfo[] fields = GetType().GetFields();
        foreach (FieldInfo field in fields)
        {
            Type type = field.FieldType;
            JsonData node = json[field.Name];
            if (node == null||"".Equals(node.ToString())) continue;
            if (type == typeof(int))
            {
                //Debug.Log(field.Name+"是整数类型,值为："+ int.Parse(node.ToString()));
                field.SetValue(this, int.Parse(node.ToString()));
            }else if(type == typeof(string))
            {
                //Debug.Log(field.Name + "是字符串类型，值为：" + node.ToString());
                field.SetValue(this, node.ToString());
            }
            else
            {
                throw new Exception("系统错误-反序列化时未指定[" + type + "]类型的赋值方式");
            }
        }
        return this;
    }
}
public class UserInfo
{
    public string userName;
    public string name;
    public string school;
    public string userNumber;
    public string grade;//年级
    public string major;//专业
    public string department;//院系
    public UserRole role;

    public static UserInfo transFromStudent(StudentInfo stuInfo)
    {
        UserInfo info = new UserInfo();
        info.role = UserRole.STUDENT;
        info.userName = stuInfo.username;
        info.name = stuInfo.name;
        info.school = stuInfo.school;
        info.userNumber = stuInfo.number;
        info.grade = stuInfo.gradeName;
        info.major = stuInfo.specialtyName;
        info.department = stuInfo.departmentName;
        return info;
    }
    public static UserInfo transFromTeacher(TeacherInfo teacherInfo)
    {
        UserInfo info = new UserInfo();
        info.role = UserRole.TEACHER;
        info.userName = teacherInfo.username;
        info.name = teacherInfo.name;
        info.school = teacherInfo.school;
        info.userNumber = teacherInfo.number;
        return info;
    }
}
//用户信息
public enum UserRole
{
    STUDENT,TEACHER
}