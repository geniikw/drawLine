using UnityEngine;
using System.Collections;
using UnityEditor;

public class MenuExtender  {

    [MenuItem("GameObject/MyCategory/MeshLine", false, 10)]
	static void CreateCustomGameObject(MenuCommand menuCommand) {
		// Create a custom game object
		GameObject go = new GameObject("Line");
		// Ensure it gets reparented if this was a context click (otherwise does nothing)
		GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

        var line = go.AddComponent<UIMeshLine>();
        line.points.Add(new LinePoint());
        line.points.Add(new LinePoint());
        line.points[0].point = new Vector2(-50, 0);
        line.points[1].point = new Vector2(50, 0);


        // Register the creation in the undo system
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
		Selection.activeObject = go;
	}


}
