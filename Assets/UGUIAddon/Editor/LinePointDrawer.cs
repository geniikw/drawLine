using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(LinePoint))]
public class LinePointDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position,label , property); 
    
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(""));

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var positionRect = new Rect(position.x, position.y-3, 180, position.height);

        EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("point"));

        //EditorGUI.DrawRect(positionRect, Color.white);
        EditorGUI.EndProperty();
    }
}
