using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ArrowClick : ObjectClickWithPointer
{
    private Color originalColor;     //ԭʼ��ɫ
    private MeshRenderer selfMesh;   //ԭʼ����

    protected override void Awake()
    {
        base.Awake();
        selfMesh = GetComponent<MeshRenderer>();
        originalColor = selfMesh.material.color;
    }

    protected override void Enter()
    {
        base.Enter();
        selfMesh.materials[0].color = Color.yellow;
    }
    protected override void Down()
    {
        base.Down();
        selfMesh.materials[0].color = originalColor;
    }
    protected override void Exit()
    {
        base.Exit();
        selfMesh.materials[0].color = originalColor;
    }
    private void OnEnable()
    {
        selfMesh.materials[0].color = originalColor;
    }
}
