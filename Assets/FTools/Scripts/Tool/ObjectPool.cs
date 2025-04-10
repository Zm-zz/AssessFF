using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Tooltip("这个对象池存储的游戏对象的预制体")]
    public GameObject prefab;

    [Tooltip("对象池初始存在对象数量。")]
    public int defaultCapacity = 10;

    [Tooltip("对象池最多能容纳多少个游戏对象。负数表示可以容纳无数个。")]
    public int capacity = -1;

    [Tooltip("对象存在的最大数量。负数表示可以容纳无数个。")]
    public int objectCountMax = -1;

    [Tooltip("从这个对象池中取出并正在使用的游戏对象。")]
    public List<GameObject> usedGameObjectList = new List<GameObject>();

    [Tooltip("存在这个对象池中没有被使用的游戏对象。")]
    public List<GameObject> unusedGameObjectList = new List<GameObject>();

    [Tooltip("这个对象池中正在使用和没有被使用的游戏对象的总数。")]
    public int TotalGameObjectCount { get => usedGameObjectList.Count + unusedGameObjectList.Count; }

    /// <summary>
    /// 从对象池获取一个对象，并返回这个对象。
    /// 如果对象池有，从对象池中取出来用。
    /// 如果对象池没有，则实例化该对象。
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public GameObject Spawn(Vector3 position,Quaternion rotation,Transform parent=null)
    {
        if (objectCountMax > 0 && usedGameObjectList.Count >= objectCountMax)
        {
            return null;
        }

        //要实例化的游戏对象
        GameObject go;

        //如果对象池中有，则从对象池中取出来用。
        if (unusedGameObjectList.Count>0)
        {
            go = unusedGameObjectList[0];
            unusedGameObjectList.RemoveAt(0);
            usedGameObjectList.Add(go);
            go.transform.localPosition = position;
            go.transform.localRotation = rotation;
            go.transform.SetParent(parent, false);
            go.SetActive(true);
        }
        //如果对象池中没有，则实例化该对象。
        else
        {
            go = Instantiate(prefab, position, rotation, parent);
            usedGameObjectList.Add(go);
        }
        //如果该游戏对象身上继承MonoBehaviour的脚本中写了名叫OnSpwan的方法，则会执行它们一次。
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);

        return go;
    }
    public GameObject Spawn(Transform parent = null)
    {
        return Spawn(Vector3.zero, Quaternion.identity, parent);
    }

    /// <summary>
    /// 隐藏指定的游戏对象，并把它回收进对象池中。
    /// </summary>
    /// <param name="go"></param>
    public void Despawn(GameObject go)
    {
        if (go == null) return;

        //遍历这个对象池中所有正在使用的游戏对象
        for (int i = 0; i < usedGameObjectList.Count; i++) 
        {
            if (usedGameObjectList[i]==go)
            {
                //如果这个对象池的容量不为负数，且容纳的游戏对象已经满，则把0号的游戏对象删除掉，确保之后新的游戏对象能放入池子中。
                if (capacity>=0&&usedGameObjectList.Count>=capacity)
                {
                    if (unusedGameObjectList.Count>0)
                    {
                        Destroy(unusedGameObjectList[0]);
                        unusedGameObjectList.RemoveAt(0);
                    }
                }

                //把游戏对象回收到对象池中
                unusedGameObjectList.Add(go);
                usedGameObjectList.RemoveAt(i);

                //如果该游戏对象身上继承MonoBehaviour的脚本写了名叫OnDespawn的方法，则在回收的时候，会执行一次。
                go.SendMessage("OnDespawn",SendMessageOptions.DontRequireReceiver);
                go.SetActive(false);
                go.transform.SetParent(transform, false);
                return;
            }
        }
    }

    /// <summary>
    /// 把通过这个对象池生成的所有游戏对象全部隐藏并放入对象池中
    /// </summary>
    public void DespawnAll()
    {
        int count = usedGameObjectList.Count;
        for (int i = 1; i <= count; i++)
        {
            Despawn(usedGameObjectList[0]);
        }
        usedGameObjectList.Clear();
    }

    /// <summary>
    /// 预先往这个对象池中加载指定数量的游戏对象
    /// </summary>
    public void Preload(int amount=1)
    {
        if (prefab == null) return;
        if (amount <= 0) return;
        ObjectPoolsManager.Instance.objectPoolsList.Add(this);

        for (int i = 1; i <= amount; i++)
        {
            if (unusedGameObjectList.Count > defaultCapacity) return;
            GameObject go = Instantiate(prefab,Vector3.zero,Quaternion.identity);
            go.SetActive(false);
            go.transform.SetParent(transform, false);
            unusedGameObjectList.Add(go);
            go.name = prefab.name;
        }
    }

    private void Awake()
    {
        Preload(defaultCapacity);
    }
}
