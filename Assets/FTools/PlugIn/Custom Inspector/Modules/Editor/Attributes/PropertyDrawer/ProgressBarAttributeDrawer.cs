using CustomInspector.Extensions;
using CustomInspector.Helpers;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace CustomInspector.Editor
{
    [CustomPropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarAttributeDrawer : PropertyDrawer
    {
        static readonly GUIStyle minLabelStyle = new(GUI.skin.label) { fontSize = 10, alignment = TextAnchor.UpperLeft };
        static readonly GUIStyle maxLabelStyle = new(GUI.skin.label) { fontSize = 10, alignment = TextAnchor.UpperRight };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PropInfo info = GetMinMax(property);
            if (info.errorMessage != null)
            {
                DrawProperties.DrawPropertyWithMessage(position, label, property, info.errorMessage, MessageType.Error);
                return;
            }

            (float min, float max) = info.getMinMax(property);

            float currentValue = Convert.ToSingle(property.GetValue());

            //Draw bar
            float betweenThresholds = (max != min) ? (currentValue - min) / (max - min) : 1; //range (0,1)
            EditorGUI.ProgressBar(position, betweenThresholds, property.name + $" ({betweenThresholds * 100}%)");

            //Draw start and end
            if(min != 0) //if not obvious
                EditorGUI.LabelField(position, min.ToString(), minLabelStyle);
            EditorGUI.LabelField(position, max.ToString(), maxLabelStyle);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            PropInfo info = GetMinMax(property);
            if (info.errorMessage != null)
                return DrawProperties.GetPropertyWithMessageHeight(label, property);
            else
                return info.barHeight;
        }

        readonly static Dictionary<PropertyIdentifier, PropInfo> storedInfos = new();
        PropInfo GetMinMax(SerializedProperty property)
        {
            PropertyIdentifier id = new(property);
            if (!storedInfos.TryGetValue(id, out PropInfo info))
            {
                info = new(property, (ProgressBarAttribute)attribute);
                storedInfos.Add(id, info);
            }
            return info;
        }
        class PropInfo
        {
            public readonly string errorMessage = null;
            public readonly float barHeight;
            public readonly Func<SerializedProperty, (float min, float max)> getMinMax;

            public PropInfo(SerializedProperty property, ProgressBarAttribute attribute)
            {
                if(property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
                {
                    errorMessage = $"ProgressBar: type '{property.propertyType}' not supported";
                    return;
                }

                barHeight = GetSize(attribute.size);

                //define getters
                SerializedProperty maxProp = attribute.maxGetter != null ? property.GetOwnerAsFinder().FindPropertyRelative(attribute.maxGetter) : null;
                SerializedProperty minProp = attribute.minGetter != null ? property.GetOwnerAsFinder().FindPropertyRelative(attribute.minGetter) : null;

                if ((attribute.maxGetter != null) == (maxProp != null) //check if has a getter, then property should be found
                    && (attribute.minGetter != null) == (minProp != null))
                {
                    Func<SerializedProperty, float> getMin;
                    Func<SerializedProperty, float> getMax;

                    //Max
                    if (attribute.maxGetter == null)
                        getMax = (prop) => attribute.max;
                    else if(maxProp.propertyType == SerializedPropertyType.Float)
                        getMax = (prop) => prop.GetOwnerAsFinder().FindPropertyRelative(attribute.maxGetter).floatValue;
                    else if(maxProp.propertyType == SerializedPropertyType.Integer)
                        getMax = (prop) => prop.GetOwnerAsFinder().FindPropertyRelative(attribute.maxGetter).intValue;
                    else
                    {
                        errorMessage = $"ProgressBar: set maximum: Property {attribute.maxGetter} is not a number";
                        return;
                    }
                    //Min
                    if (attribute.minGetter == null)
                        getMin = (prop) => attribute.min;
                    else if (minProp.propertyType == SerializedPropertyType.Float)
                        getMin = (prop) => prop.GetOwnerAsFinder().FindPropertyRelative(attribute.minGetter).floatValue;
                    else if (minProp.propertyType == SerializedPropertyType.Integer)
                        getMin = (prop) => prop.GetOwnerAsFinder().FindPropertyRelative(attribute.minGetter).intValue;
                    else
                    {
                        errorMessage = $"ProgressBar: set minimum: Property {attribute.minGetter} is not a number";
                        return;
                    }

                    getMinMax = (prop) => (getMin(prop), getMax(prop));
                }
                else //properties were not found, so they are not serializable
                {
                    //Check if existing
                    //Max
                    if(attribute.maxGetter != null)
                    {
                        DirtyValue maxValue;
                        try
                        {
                            maxValue = DirtyValue.GetOwner(property).FindRelative(attribute.maxGetter);
                        }
                        catch (Exception e)
                        {
                            errorMessage = e.Message;
                            return;
                        }
                        //Check type
                        if (!typeof(float).IsAssignableFrom(maxValue.Type))
                        {
                            errorMessage = $"ProgressBar: set minimum: Property {attribute.maxGetter} is not a number";
                            return;
                        }
                    }
                    //Min
                    if (attribute.minGetter != null)
                    {
                        DirtyValue minValue;
                        try
                        {
                            minValue = DirtyValue.GetOwner(property).FindRelative(attribute.minGetter);
                        }
                        catch (Exception e)
                        {
                            errorMessage = e.Message;
                            return;
                        }
                        //Check type
                        if(!typeof(float).IsAssignableFrom(minValue.Type))
                        {
                            errorMessage = $"ProgressBar: set minimum: Property {attribute.minGetter} is not a number";
                            return;
                        }
                    }

                    //set functions
                    getMinMax = (prop) =>
                    {
                        prop.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                        DirtyValue owner = DirtyValue.GetOwner(property);
                        if(attribute.maxGetter == null) //only min
                            return ((float)owner.FindRelative(attribute.minGetter).GetValue(), attribute.max);
                        else if (attribute.minGetter == null) //only max
                            return (attribute.min, (float)owner.FindRelative(attribute.maxGetter).GetValue());
                        else //both
                            return ((float)owner.FindRelative(attribute.minGetter).GetValue(), (float)owner.FindRelative(attribute.maxGetter).GetValue());
                    };
                }
            }
            float GetSize(Size size)
            {
                return size switch
                {
                    Size.small => EditorGUIUtility.singleLineHeight,
                    Size.medium => 30,
                    Size.big => 40,
                    Size.max => 50,
                    _ => throw new System.NotImplementedException(size.ToString()),
                };
            }
        }
    }
}
