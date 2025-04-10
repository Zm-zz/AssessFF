using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private void Update()
    {
        Vector3 toCameraVector = Camera.main.transform.position - transform.position;
        Quaternion newRotation = Quaternion.LookRotation(toCameraVector, Vector3.up);
        transform.rotation = newRotation;
    }
}
