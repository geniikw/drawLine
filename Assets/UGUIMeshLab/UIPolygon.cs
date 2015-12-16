using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// todo : 추가기능 시계방향으로 만들기.
/// </summary>

public class UIPolygon : MaskableGraphic, IMeshModifier
{
    public List<PolygonVertexInfo> vertexInfoList = new List<PolygonVertexInfo>();
    [Range(0f,360f)]
    public float offset = 0f;

    public bool innerPolygon = false;
    [Header("innerPolygon 옵션에서 제로점과 가까울시 width의 예외처리가 되어있지 않음. ")][Range(0f,1f)]
    public float width = 1f;
    public bool vertexColorFlag = false;
    
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
        int count = vertexInfoList.Count;
        if (count < 3)
            return;//3개서부터 보임 

        for (int n = 0; n < vertexInfoList.Count; n++)
        {
            vh.AddVert(getRadiusPosition(vertexInfoList[n], n), checkVertexColor(vertexInfoList[n].color) ,Vector2.zero);
        }

        if (!innerPolygon)
        {
            for (int n = 0; n < count - 2; n++)
            {
                vh.AddTriangle(0, n + 1, n + 2);
            }
        }
        else
        {
            for (int n = 0; n < count; n++)
            {
                vh.AddVert(getRadiusPosition(vertexInfoList[n], n, width), checkVertexColor(vertexInfoList[n].color), Vector2.zero);
            }
            for (int n = 0; n < count; n++)
            {
                vh.AddTriangle(n, (n + 1) % count, count + (1 + n) % count);
                vh.AddTriangle(n, n + count, count + (1 + n) % count);
            }
        }
    }
    Vector3 getRadiusPosition(PolygonVertexInfo info, int index, float scale=1f) 
    {
        if (vertexInfoList.Count < 3)
            return Vector3.zero;

        float width = rectTransform.rect.width / 2 * info.length ;
        float height = rectTransform.rect.height / 2 * info.length ;
        
        float angleUnit = 2f * Mathf.PI / vertexInfoList.Count;
        float offsetToAngle = offset / 360 * Mathf.PI * 2;

        Vector3 result = new Vector3(width * Mathf.Cos(angleUnit * index + offsetToAngle),  height* Mathf.Sin(angleUnit* index + offsetToAngle));
        return result * scale;
    }

    Color checkVertexColor(Color vertexColor)
    {
        if (vertexColorFlag)
        {
            return vertexColor;
        }
        else
        {
            return color;
        }
    }
}
