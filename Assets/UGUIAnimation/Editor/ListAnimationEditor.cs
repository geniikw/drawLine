using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(ListAnimation))]
public class ListAnimationEditor : Editor {

    ListAnimation owner { get { return target as ListAnimation; } }
    SerializedProperty m_list;
       

    void OnEnable()
    {
        m_list = serializedObject.FindProperty("list");
    }
    bool isFold = false;
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
                
        isFold = EditorGUILayout.PropertyField(m_list);
        if (isFold)
        {
            for (int n = 0; n < m_list.arraySize; n++)
            {
                EditorGUILayout.PropertyField(m_list.GetArrayElementAtIndex(n));
            }
        }

        if (GUILayout.Button("findchild"))
        {
            FindChild();
        }
    }

    void FindChild()
    {
        owner.list.Clear();
        for (int n = 0; n < owner.transform.childCount; n++)
        {
            owner.list.Add(owner.transform.GetChild(n).GetComponent<ParentAnimation>());
        }
    }
}
