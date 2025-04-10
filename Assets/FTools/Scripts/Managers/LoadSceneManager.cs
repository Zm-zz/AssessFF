using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
/// <summary>
/// 切换场景的管理器
/// </summary>
public class LoadSceneManager : SingletonPatternBase<LoadSceneManager>
{

    /// <summary>
    /// 重新切换到当前场景
    /// </summary>
    public void LoadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// 切换到下一个场景
    /// </summary>
    public void LoadNextScene(bool isCyclical=false)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (buildIndex>SceneManager.sceneCountInBuildSettings-1)
        {
            if (isCyclical)
                buildIndex = 0;
            else
            {
                Debug.LogWarning($"加载场景失败！要加载的场景的索引是{buildIndex}，越界了！");
                return;
            }
        }

        SceneManager.LoadScene(buildIndex);

        return;
    }

    /// <summary>
    /// 切换到上一个场景。
    /// </summary>
    public void LoadPreviousScene(bool isCyclical = false)
    {
        int buildIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (buildIndex <0)
        {
            if (isCyclical)
                buildIndex = SceneManager.sceneCountInBuildSettings - 1;
            else
            {
                Debug.LogWarning($"加载场景失败！要加载的场景的索引是{buildIndex}，索引不能为负数！");
                return;
            }
        }

        SceneManager.LoadScene(buildIndex);

        return;
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    public void LoadSceneAsync(string sceneName,UnityAction<float> loading=null,UnityAction<AsyncOperation> completed=null,bool setActiveAfterCompleted=true,LoadSceneMode mode=LoadSceneMode.Single)
    {
        MonoManager.Instance.StartCoroutine(LoadSceneCoroutine(sceneName,loading,completed, setActiveAfterCompleted,mode));
    }
    IEnumerator LoadSceneCoroutine(string sceneName, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null, bool setActiveAfterCompleted = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        //开始加载资源
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, mode);

        asyncOperation.allowSceneActivation = false;

        //等待资源加载完毕
        while (asyncOperation.progress<0.9f)
        {
            loading?.Invoke(asyncOperation.progress);
            yield return null;
        }

        //当asyncOperation.allowSceneActivation为false，则asyncOperation.progress最多只能到达0.9，我们人为把它凑成1，可以方便外部进度条的显示。
        loading?.Invoke(1);

        asyncOperation.allowSceneActivation = setActiveAfterCompleted;

        //加载资源完毕后执行的逻辑
        completed?.Invoke(asyncOperation);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    public void LoadSceneAsync(int sceneIndex, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null, bool setActiveAfterCompleted = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        MonoManager.Instance.StartCoroutine(LoadSceneCoroutine(sceneIndex, loading, completed, setActiveAfterCompleted, mode));
    }
    IEnumerator LoadSceneCoroutine(int sceneIndex, UnityAction<float> loading = null, UnityAction<AsyncOperation> completed = null, bool setActiveAfterCompleted = true, LoadSceneMode mode = LoadSceneMode.Single)
    {
        yield return new WaitForSeconds(0.1f);
        //开始加载资源
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex, mode);
        asyncOperation.allowSceneActivation = false;
        //等待资源加载完毕
        while (asyncOperation.progress < 0.9f)
        {
            loading?.Invoke(asyncOperation.progress);
            yield return null;
        }
        //当asyncOperation.allowSceneActivation为false，则asyncOperation.progress最多只能到达0.9，我们人为把它凑成1，可以方便外部进度条的显示。
        loading?.Invoke(1);

        asyncOperation.allowSceneActivation = setActiveAfterCompleted;

        //加载资源完毕后执行的逻辑
        completed?.Invoke(asyncOperation);
    }

}
