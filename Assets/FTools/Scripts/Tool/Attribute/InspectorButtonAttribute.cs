using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Method)]
public class InspectorButtonAttribute : Attribute
{
    public readonly string Name;

    public InspectorButtonAttribute()
    {
    }

    public InspectorButtonAttribute(string name)
    {
        Name = name;
    }
}