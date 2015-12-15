using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// just reference for vertex line
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VertexLine : MonoBehaviour
{
    MeshFilter meshfilter { get { return GetComponent<MeshFilter>(); } }
    MeshRenderer meshRender { get { return GetComponent<MeshRenderer>(); } }

    public int Count { get { return points.Count; } }
    public Vector3 this[int index]
    {
        get
        {
            return points[index];
        }
        set
        {
            points[index] = value;
            VertexUpdate();
        }
    }

    [SerializeField]
    List<Vector3> points = new List<Vector3>();
    Mesh mesh;
    /// <summary>
    /// points가 변경될때마다 호출되어야함...
    /// </summary>
    public void VertexUpdate()
    {
        if (!mesh)
        {
            if (!meshfilter.sharedMesh)
            {
                meshfilter.sharedMesh = new Mesh();
            }
            meshfilter.hideFlags = HideFlags.DontSave;
            mesh = meshfilter.sharedMesh;
        }
        int[] indices = new int[points.Count];
        mesh.vertices = points.ToArray();   
        for (int n = 0; n < points.Count; n++)
        {
            indices[n] = n;
        }
        mesh.SetIndices(indices, MeshTopology.LineStrip, 0);
    }

    //GL 함수를 사용한 low-level drawLine
    void DrawGLLine(Vector3 p0, Vector3 p1, Color color, Material mat)
    {
        mat.SetPass(0);
        GL.PushMatrix();

        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);

        GL.Color(color);
        GL.Vertex(p0);
        GL.Vertex(p1);

        GL.End();
        GL.PopMatrix();
    }
    Material mat;
    void CreateMaterial()
    {
        var shader = Shader.Find("Hidden/Internal-Colored");
        mat = new Material(shader);
        mat.hideFlags = HideFlags.HideAndDontSave;
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        // Turn off backface culling, depth writes, depth test.
        mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        mat.SetInt("_ZWrite", 0);
        mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
    }
}