using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 对象池管理器。可以通过这个管理器从对象池生成游戏对象，也可以回收游戏对象进对象池。
/// </summary>
public class ObjectPoolsManager : SingletonPatternMonoAutoBase<ObjectPoolsManager>
{
    [Tooltip("所有对象池的父物体")]
    GameObject poolsParent;

    [Tooltip("所有对象池的共同父物体在Hierarchy窗口的名字")]
    string poolsParentName = "ObjectPools";

    [Tooltip("当前所有对象池的列表")]
    public List<ObjectPool> objectPoolsList = new List<ObjectPool>();

    [Tooltip("用来表示游戏对象所属的对象池")]
    public Dictionary<GameObject, ObjectPool> objectsDictionary = new Dictionary<GameObject, ObjectPool>();

    /// <summary>
    /// 从对象池生成游戏对象。
    /// 如果对象池有，从对象池中取出来用。
    /// 如果对象池没有，实例化该对象。
    /// </summary>
    public GameObject Spawn(GameObject prefab,Vector3 position,Quaternion rotation,Transform parent=null)
    {
        if (prefab == null) return null;

        //如果场景中没有对象池的父物体，则生成一个空物体，作为所有对象池的父物体
        CreatePoolsParentIfNull();

        //先通过预制体来查找它所属的小对象池，如果找到了，则返回这个小对象池。
        //如果找不到，则创建一个小对象池，用来存放这种预制体。
        ObjectPool objectPool = FindPoolByPrefabOrCreatePool(prefab);
        GameObject go = objectPool.Spawn(position, rotation, parent);
        if (go == null) return null;
        objectsDictionary.Add(go, objectPool);
        return go;
    }
    public GameObject Spawn(GameObject prefab, Transform parent = null)
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity, parent);
    }

    public void Despawn(GameObject go,float delayTime=0)
    {
        if (go == null) return;

        StartCoroutine(DespawnCoroutine(go,delayTime));
    }
    IEnumerator DespawnCoroutine(GameObject go, float delayTime = 0)
    {
        //等待指定秒数
        if (delayTime > 0)
            yield return new WaitForSeconds(delayTime);

        if (objectsDictionary.TryGetValue(go, out ObjectPool pool))
        {
            objectsDictionary.Remove(go);

            pool.Despawn(go);
        }
        else
        {
            //获取这个游戏对象所属的对象池
            pool = FindPoolByUsedGameObject(go);

            if (pool != null)
            {
                pool.Despawn(go);
            }
        }
    }

    ObjectPool FindPoolByUsedGameObject(GameObject go)
    {
        if (go == null) return null;

        for (int i = 0; i < objectPoolsList.Count; i++)
        {
            ObjectPool pool = objectPoolsList[i];

            for (int j = 0; j < pool.usedGameObjectList.Count; j++)
            {
                if (pool.usedGameObjectList[j] == go)
                    return pool;
            }
        }

        return null;
    }

    //如果场景中没有对象池的父物体，则生成一个空物体，作为所有对象池的父物体。
    void CreatePoolsParentIfNull()
    {
        poolsParent = FindObjectOfType<ObjectPoolsManager>().gameObject;

        if (poolsParent==null)
        {
            objectPoolsList.Clear();
            objectsDictionary.Clear();

            poolsParent = new GameObject(poolsParentName);
        }
    }

    //先通过预制体来查找它所属的小对象池，如果找到了，则返回这个小对象池。
    //如果找不到，则创建一个小对象池，用来存放这种预制体。
    ObjectPool FindPoolByPrefabOrCreatePool(GameObject prefab)
    {
        //确保大对象池是存在的
        CreatePoolsParentIfNull();

        //查找并返回该预制体对数的对象池
        ObjectPool objectPool= FindPoolByPrefab(prefab);

        if (objectPool==null)
        {
            objectPool = new GameObject($"ObjectPool_{prefab.name}").AddComponent<ObjectPool>();
            objectPool.prefab = prefab;
            objectPool.transform.SetParent(poolsParent.transform);
            objectPoolsList.Add(objectPool);
        }

        return objectPool;
    }

    ObjectPool FindPoolByPrefab(GameObject prefab)
    {
        if (prefab == null) return null;

        for (int i = 0; i < objectPoolsList.Count; i++)
        {
            if (objectPoolsList[i].prefab == prefab)
                return objectPoolsList[i];
        }

        return null;
    }
}
