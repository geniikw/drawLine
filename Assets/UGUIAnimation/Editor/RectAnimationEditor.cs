using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(RectAnimation))]
public class RectAnimationEditor : Editor {
    RectAnimation owner { get { return (RectAnimation)target; } }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("PlayAnimationTest"))
        {
            if (EditorApplication.isPlaying)
            {
                owner.StartCoroutine(owner.Play());
            }
            else
            {
                IEnumerator it = owner.Play();
                while (it.MoveNext()) ;
            }
        }
    }
}
