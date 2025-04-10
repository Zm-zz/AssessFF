#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class GlobalMeshCollider : MonoBehaviour
{
    [MenuItem("MyTools/�����ײ��")]
    public static void Add()
    {
        //Ѱ��Hierarchy��������е�MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            //�������Ҫ�������������û��������룬unity�ǲ��������༭���иĶ��ģ���Ȼ�������ֱ���л������ı��ǲ�������
            //��  ��������������  ��������ĺ� �Լ�����ֶ��޸��³����������״̬ �ڱ���ͺ��� 
            Undo.RecordObject(t, t.gameObject.name);

            MeshCollider meshCollider = t.gameObject.GetComponent<MeshCollider>();
            if (meshCollider == null)
            {
                t.gameObject.AddComponent<MeshCollider>();
            }

            //�൱������ˢ���� ��Ȼunity��ʾ���滹��֪���Լ��Ķ�����������  �����������ʾ֮ǰ�Ķ���
            EditorUtility.SetDirty(t);
        }
        Debug.Log("Add Succed");
    }

    [MenuItem("MyTools/�Ƴ���ײ��")]
    public static void Remove()
    {
        //Ѱ��Hierarchy��������е�MeshRenderer
        var tArray = Resources.FindObjectsOfTypeAll(typeof(MeshRenderer));
        for (int i = 0; i < tArray.Length; i++)
        {
            MeshRenderer t = tArray[i] as MeshRenderer;
            //�������Ҫ�������������û��������룬unity�ǲ��������༭���иĶ��ģ���Ȼ�������ֱ���л������ı��ǲ�������
            //��  ��������������  ��������ĺ� �Լ�����ֶ��޸��³����������״̬ �ڱ���ͺ��� 
            Undo.RecordObject(t, t.gameObject.name);

            MeshCollider meshCollider = t.gameObject.GetComponent<MeshCollider>();
            if (meshCollider != null)
            {
                DestroyImmediate(meshCollider);
            }

            //�൱������ˢ���� ��Ȼunity��ʾ���滹��֪���Լ��Ķ�����������  �����������ʾ֮ǰ�Ķ���
            EditorUtility.SetDirty(t);
        }
        Debug.Log("remove Succed");
    }
}

#endif