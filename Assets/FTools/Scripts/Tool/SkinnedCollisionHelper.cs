//using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using UnityEngine;

/// <summary>
/// SkinnedMesh的碰撞体修正
/// </summary>
[RequireComponent(typeof(MeshCollider))]
public class SkinnedCollisionHelper : MonoBehaviour
{
    SkinnedMeshRenderer meshRenderer;
    MeshCollider col;
    float timer;
    public static float maxInterval = 0.5f;

    private void Awake()
    {
        col = GetComponent<MeshCollider>();
        timer = maxInterval;
    }

    private void Update()
    {
        if (timer <= 0)
        {
            ColliderUpdate();
            timer = maxInterval;
        }
        timer -= Time.deltaTime;    
    }

    //更新方法
    private void ColliderUpdate()
    {
        Mesh colliderMesh = new Mesh();
        if(TryGetComponent(out meshRenderer))
        {
            meshRenderer.BakeMesh(colliderMesh,true);
        }
        col.sharedMesh = colliderMesh; //将新的mesh赋给meshcollider
    }
}
