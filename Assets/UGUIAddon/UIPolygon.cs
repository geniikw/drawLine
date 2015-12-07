using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIPolygon : MaskableGraphic, IMeshModifier
{
    public List<PolygonVertexInfo> vertexInfoList = new List<PolygonVertexInfo>();
    [Range(0f,360f)]
    public float offset = 0f;

    [System.Serializable]
    public class PolygonVertexInfo
    {
        public Color color = Color.white;
        [Range(0f,1f)]
        public float length = 1f;
    }

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
        vh.Clear();
        for(int n =0; n < vertexInfoList.Count; n++)
        {
            UIVertex vertex = new UIVertex();
            vertex.color = vertexInfoList[n].color;
            vertex.position = getRadiusPosition(vertexInfoList[n], n);
            vh.AddVert(vertex);
        }
        
        for(int n =0;n < vertexInfoList.Count-2; n++)
        {
            vh.AddTriangle(0, n + 1, n + 2);
        }
    }
    Vector3 getRadiusPosition(PolygonVertexInfo info, int index) 
    {
        if (vertexInfoList.Count < 3)
            return Vector3.zero;

        float width = rectTransform.rect.width / 2 * info.length ;
        float height = rectTransform.rect.height / 2 * info.length ;

        float angleUnit = 2f * Mathf.PI / vertexInfoList.Count;
        float offset2 = offset / 360 * Mathf.PI * 2;

        Vector3 result = new Vector3(width * Mathf.Cos(angleUnit * index + offset2),  height* Mathf.Sin(angleUnit* index + offset2)) ; ;
        return result;
    }


}
