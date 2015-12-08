using UnityEngine;
using System.Collections;

using UnityEngine.UI;
using System.Collections.Generic;

public class UIVertexLine2 : MaskableGraphic, IMeshModifier
{
   
    public AnimationCurve curve;
    public List<LinePoint> list = new List<LinePoint>();

    public void ModifyMesh(VertexHelper vh)
    {
        EditMesh(vh);
    }

    public void ModifyMesh(Mesh mesh)
    {
        using (var vh = new VertexHelper(mesh))
        {
            EditMesh(vh);
            vh.FillMesh(mesh);
        }
    }

    void EditMesh(VertexHelper vh)
    {
        
    }
}
