using System.Collections;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 资源管理器
/// </summary>
public class ResourcesManager : SingletonPatternBase<ResourcesManager>
{
    /// <summary>
    /// 同步加载Resources文件夹中指定类型的资源。
    /// 如果有多个相同类型，且相同路径的资源，则只会返回找到的第一个资源。
    /// </summary>
    /// <typeparam name="T">要加载的资源的类型</typeparam>
    /// <param name="path">资源的路径</param>
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    /// <summary>
    /// <para>异步加载Resources文件夹中指定类型的资源</para>
    /// <para>如果有多个相同类型，且相同路径的资源，则只会加载找到的第一个资源。</para>
    /// </summary>
    /// <typeparam name="T">加载的资源的类型</typeparam>
    /// <param name="path">要加载的资源的路径</param>
    /// <param name="callback">资源加载完毕后要执行的逻辑</param>
    public void LoadAsync<T>(string path, UnityAction<T> callback = null) where T : Object
    {
        MonoManager.Instance.StartCoroutine(LoadAsyncCoroutine(path, callback));
    }
    IEnumerator LoadAsyncCoroutine<T>(string path, UnityAction<T> callback = null) where T : Object
    {
        //开始加载资源
        ResourceRequest resourceRequest = Resources.LoadAsync<T>(path);
        //等待资源加载完毕
        yield return resourceRequest;
        callback?.Invoke(resourceRequest.asset as T);
    }

    /// <summary>
    /// 异步卸载资源，往往在切换场景的时候使用。
    /// </summary>
    public void UnloadUnusedAssets(UnityAction callback = null)
    {
        MonoManager.Instance.StartCoroutine(UnloadUnusedAssetsCoroutine(callback));
    }
    IEnumerator UnloadUnusedAssetsCoroutine(UnityAction callback = null)
    {
        //开始卸载资源
        AsyncOperation asyncOperation = Resources.UnloadUnusedAssets();

        //等待资源卸载
        while (asyncOperation.progress < 1)
        {
            yield return null;
        }

        //资源卸载完毕后执行的逻辑
        callback?.Invoke();
    }
}
