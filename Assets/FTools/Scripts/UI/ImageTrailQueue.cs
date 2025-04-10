using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ImageTrailQueue : MonoBehaviour
{
    public Queue<GameObject> trails;
    public int traliCount;
    public Transform trans;
    public float disDelta;

    private void Awake()
    {
        if (trans == null) trans = transform.parent;
        trails = new Queue<GameObject>();
    }

    private void Start()
    {
        for (int i = 0; i < traliCount; i++)
        {
            GameObject obj = ObjectPoolsManager.Instance.Spawn(gameObject, trans);
            obj.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);
            obj.GetComponent<Image>().color = FMethod.SetColor_A(obj.GetComponent<Image>().color, 0.1f);
            Destroy(obj.GetComponent<ImageTrailQueue>());
            obj.transform.position = transform.position;
            trails.Enqueue(obj);
        }
    }

    private void FixedUpdate()
    {
        float dis = Vector3.Distance(trails.Last().transform.position, transform.position);
        Vector3 dir = transform.position - trails.Last().transform.position;
        if (dis > disDelta)
        {
            int count = (int)(dis / disDelta) + 1;
            if (count > traliCount) count = traliCount;
            for (int i = 0; i < count; i++)
            {
                GameObject trail = trails.Dequeue();
                trail.transform.position = transform.position + dir.normalized * disDelta * (count - i);
                trails.Enqueue(trail);
            }
        }
    }

    public void ResetTrail()
    {
        foreach (var trail in trails)
        {
            trail.transform.position = transform.position;
        }
    }
}
