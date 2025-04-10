using DG.Tweening;
using HighlightPlus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HighlightEffect), typeof(MeshCollider))]
public class InteractionModel : ObjectClickWithRay
{
    public bool enableHighLight;
    public HighlightEffect highlight;


    protected override void AlwaysDown()
    {
        base.AlwaysDown();

        Debug.Log($"点击物体：<color=green>{gameObject.name}</color>");

        ModelCtrl.Instance.currClickModel = this;
    }

    protected override void AlwaysUp()
    {
        base.AlwaysUp();
    }

    protected override void AlwaysEnter()
    {
        base.AlwaysEnter();

        if (enableHighLight)
        {
            highlight.SetHighlighted(true);
        }

        ModelCtrl.Instance.currEnterModel = this;

    }

    protected override void AlwaysExit()
    {
        base.AlwaysExit();

        if (enableHighLight)
        {
            highlight.SetHighlighted(false);
        }
    }

    protected override void Enter()
    {
        //移入事件
    }

    protected override void Exit()
    {
        //移出事件
    }

    protected override void Down()
    {
        //按下事件
    }

    protected override void Up()
    {
        //抬起事件
    }
}
