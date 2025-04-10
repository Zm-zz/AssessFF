using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageTrailCreate : MonoBehaviour
{
    public bool isOn;
    public float createInterval;
    public float fadeInterval;
    float timer;
    public Transform parent;

    List<GameObject> objs;

    private void Awake()
    {
        if (parent == null) parent = transform.parent;
        objs = new List<GameObject>();
    }

    private void FixedUpdate()
    {
        if (isOn)
        {
            timer += Time.deltaTime;
            if (timer >= createInterval)
            {
                timer = 0;
                CreateTrail();
            }
        }
    }

    void CreateTrail()
    {
        GameObject obj = ObjectPoolsManager.Instance.Spawn(gameObject, parent);
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(gameObject.GetComponent<RectTransform>().rect.width, gameObject.GetComponent<RectTransform>().rect.height);
        Destroy(obj.GetComponent<ImageTrailCreate>());
        obj.transform.position = transform.position;
        obj.GetComponent<Image>().CrossFadeAlpha(0, fadeInterval, false);
        objs.Add(obj);
        StartCoroutine(DestroyTrail(obj));
    }

    IEnumerator DestroyTrail(GameObject obj)
    {
        yield return new WaitForSeconds(fadeInterval);
        if (objs.Contains(obj)) objs.Remove(obj);
        ObjectPoolsManager.Instance.Despawn(obj);
    }

    public void ClearTrail()
    {
        foreach (var obj in objs)
        {
            ObjectPoolsManager.Instance.Despawn(obj);
        }
        objs.Clear();
        StopAllCoroutines();
    }

    private void OnEnable()
    {
        ClearTrail();
    }
}
