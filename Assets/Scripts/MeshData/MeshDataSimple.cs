using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomReticle
{
    public class MeshDataSimple : IMeshData
    {
        private List<Vector3> m_Vertices = new List<Vector3>(Capacity);

        private List<Vector3> m_Normals = new List<Vector3>(Capacity);

        private List<Vector2> m_UVs = new List<Vector2>(Capacity);

        private List<Color> m_Colors = new List<Color>(Capacity);

        private List<int> m_Triangles = new List<int>(Capacity);

        private const int Capacity = 1024;

        public void AppendVertex(Vector3 position, Vector3 vertexNormal, Vector2 uv, Color color)
        {
            m_Vertices.Add(position);
            m_Normals.Add(vertexNormal);
            m_UVs.Add(uv);
            m_Colors.Add(color);
        }

        public void AppendTriangleVertexIndex(int vertexIndex)
        {
            m_Triangles.Add(vertexIndex);
        }

        public int GetVertexCount()
        {
            return m_Vertices.Count;
        }

        public void Clear()
        {
            m_Vertices.Clear();
            m_Normals.Clear();
            m_UVs.Clear();
            m_Colors.Clear();
            m_Triangles.Clear();
        }

        public void StartNew(Mesh mesh)
        {
            mesh.Clear(false);
            Clear();
        }

        public void Apply(Mesh mesh)
        {
            mesh.SetVertices(m_Vertices);
            mesh.SetNormals(m_Normals);
            mesh.SetUVs(0, m_UVs);
            mesh.SetColors(m_Colors);
            mesh.SetTriangles(m_Triangles, 0);
        }
    }
}
