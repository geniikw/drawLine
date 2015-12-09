using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(LinePoint))]
public class LinePointDrawer : PropertyDrawer {

    public bool isFold = true;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;
        
        isFold = !EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 16), !isFold, label, false);
        if (!isFold)
        {
            var next = property.FindPropertyRelative("isNextCurve").boolValue;
            var prv = property.FindPropertyRelative("isPrvCurve").boolValue;

            var positionRect = new Rect(position.x + 65, position.y + 16, 140f, 16f);
            var positionPrvBool = new Rect(position.x + 10, position.y + 16, 30f, 16f);
            var positionNextBool = new Rect(position.x + 215, position.y + 16, 30f, 16f);

            if (next)
            {
                positionNextBool.y += 16;
            }
            if (prv)
            {
                positionRect.y += 16;
                positionNextBool.y += 16;
            }



            //EditorGUI.DrawRect(positionNextBool, Color.black);
            //EditorGUI.DrawRect(positionPrvBool, Color.black);


            EditorGUI.BeginProperty(position, label, property);

            EditorGUIUtility.labelWidth = 70f;
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Native), label);
            
            EditorGUI.PropertyField(positionNextBool,property.FindPropertyRelative("isNextCurve"),GUIContent.none);
            EditorGUI.PropertyField(positionPrvBool, property.FindPropertyRelative("isPrvCurve"), GUIContent.none);
            positionRect.x -= 20; GUI.Label(positionRect, "point"); positionRect.x += 20;

            EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("point"), GUIContent.none);
            if (next)
            {
                positionRect.y += 16;
                positionRect.x -= 20; GUI.Label(positionRect, "next"); positionRect.x += 20;
                EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("nextCurvePoint"), GUIContent.none); positionRect.y -= 16;
            }
            if (prv)
            {
                positionRect.y -= 16;
                positionRect.x -= 20; GUI.Label(positionRect, "prv"); positionRect.x += 20;
                EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("prvCurvePoint"), GUIContent.none);
            }

            EditorGUI.EndProperty();
        }

      
        EditorGUI.indentLevel = indent;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (isFold)
            return base.GetPropertyHeight(property, label);

        float propertyHeight = 16f;
        if (property.FindPropertyRelative("isNextCurve").boolValue)
        {
            propertyHeight += 16f;
        }

        if (property.FindPropertyRelative("isPrvCurve").boolValue)
        {
            propertyHeight += 16f;
        }
        
        return base.GetPropertyHeight(property, label) + propertyHeight;
    }

}
