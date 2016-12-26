using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


[CustomPropertyDrawer(typeof(LinePoint))]
public class LinePointDrawer : PropertyDrawer
{

    //todo(solved) : 리스트에서 1개를 접고 피면 전부다 열림;=> 이 PropertyDrawer가 모든 맴버가 공유하는 바람에 문제가 생김 
    //프로퍼티에 맴버로 추가해서 해결 에디터에 필요한거라 인게임 데이터 콘테이너에 넣는게 좀 이상하지만 #if 로 대충 떼움.

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        property.FindPropertyRelative("isFold").boolValue = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, 16), property.FindPropertyRelative("isFold").boolValue, label, true);
        if (!property.FindPropertyRelative("isFold").boolValue)
        {

            var next = property.FindPropertyRelative("isNextCurve").boolValue;
            var prv = property.FindPropertyRelative("isPrvCurve").boolValue;

            var positionRect = new Rect(position.x + 65, position.y + 16, 140f, 16f);
            var positionPrvBool = new Rect(position.x + 10, position.y + 16, 30f, 16f);
            var positionNextBool = new Rect(position.x + 215, position.y + 16, 30f, 16f);

            var positionDivideCount = positionRect;
            var positionWidth = positionRect;
            var positionAngle = positionRect;

            positionDivideCount.y += 16;
            positionDivideCount.width += 16;

            positionWidth.y += 32;
            positionWidth.width += 16;

            positionAngle.y += 48;
            positionAngle.width += 16;

            if (next)
            {
                positionNextBool.y += 16;
                positionDivideCount.y += 16;
                positionWidth.y += 16;
                positionAngle.y += 16;
            }
            if (prv)
            {
                positionRect.y += 16;
                positionNextBool.y += 16;
                positionDivideCount.y += 16;
                positionWidth.y += 16;
                positionAngle.y += 16;
            }
            EditorGUI.PropertyField(positionDivideCount, property.FindPropertyRelative("nextCurveDivideCount"));
            EditorGUI.PropertyField(positionWidth, property.FindPropertyRelative("width"), false);

            EditorGUI.PropertyField(positionAngle, property.FindPropertyRelative("angle"), false);



            EditorGUI.BeginProperty(position, label, property);

            EditorGUIUtility.labelWidth = 70f;
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Native), label); //맴버의 이름인데 필요없을듯...?

            EditorGUI.PropertyField(positionNextBool, property.FindPropertyRelative("isNextCurve"), GUIContent.none);
            EditorGUI.PropertyField(positionPrvBool, property.FindPropertyRelative("isPrvCurve"), GUIContent.none);
            positionRect.x -= 20; GUI.Label(positionRect, "point"); positionRect.x += 20;

            EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("point"), GUIContent.none);
            if (next)
            {
                positionRect.y += 16;
                positionRect.x -= 20; GUI.Label(positionRect, "next"); positionRect.x += 20;  ///좀 병신같은데... 귀찮으니 그냥 씀 어짜피 에디터 코드라 상관 없을듯.
                EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("nextCurveOffset"), GUIContent.none); positionRect.y -= 16;
            }
            else
            {
                property.FindPropertyRelative("nextCurveOffset").vector2Value = Vector2.zero;
            }

            if (prv)
            {
                positionRect.y -= 16;
                positionRect.x -= 20; GUI.Label(positionRect, "prv"); positionRect.x += 20;
                EditorGUI.PropertyField(positionRect, property.FindPropertyRelative("prvCurveOffset"), GUIContent.none);
            }
            else
            {
                property.FindPropertyRelative("prvCurveOffset").vector2Value = Vector2.zero;
            }


            EditorGUI.EndProperty();
        }

        EditorGUI.indentLevel = indent;
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.FindPropertyRelative("isFold").boolValue)
            return base.GetPropertyHeight(property, label);

        float propertyHeight = 64f;
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