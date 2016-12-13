using UnityEngine;
using System.Collections;
using UnityEditor;

namespace UGUIAnimation
{
    public class MenuExtender
    {

        [MenuItem("GameObject/UI/MeshLine", false, 10)]
        static void CreateCustomGameObject(MenuCommand menuCommand)
        {
            var go = new GameObject();

            var line = go.AddComponent<UIMeshLine>();
            
            line.AddPoint(new LinePoint(new Vector2(-50, 0)));
            line.AddPoint(new LinePoint(new Vector2(50, 0)));

            
            go.name = "MeshLine";

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
        }


    }
}

