using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace CustomReticle
{
    public class MeshDataAdvanced : IMeshData
    {
        private List<MeshVertex> m_Vertices = new List<MeshVertex>(Capacity);

        private List<ushort> m_Triangles = new List<ushort>(Capacity);

        private const int Capacity = 1024;

        private static readonly VertexAttributeDescriptor[] layout;

        static MeshDataAdvanced()
        {
            layout = new VertexAttributeDescriptor[]
            {
                new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.UNorm8, 4),
                new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
            };
        }

        public void AppendTriangleVertexIndex(int vertexIndex)
        {
            m_Triangles.Add((ushort)vertexIndex);
        }

        public void AppendVertex(Vector3 position, Vector3 vertexNormal, Vector2 uv, Color color)
        {
            m_Vertices.Add(new MeshVertex(position, color, uv));
        }

        public void Apply(Mesh mesh)
        {
            // Vertex Buffer.

            int vertexCount = GetVertexCount();
            mesh.SetVertexBufferParams(vertexCount, layout);
            mesh.SetVertexBufferData(m_Vertices, 0, 0, vertexCount);

            // Index Buffer.

            int trianglesCount = m_Triangles.Count;
            mesh.SetIndexBufferParams(trianglesCount, IndexFormat.UInt16);
            mesh.SetIndexBufferData(m_Triangles, 0, 0, trianglesCount, MeshUpdateFlags.DontValidateIndices);

            // Sub Mesh.

            mesh.subMeshCount = 1;
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, trianglesCount, MeshTopology.Triangles));
        }

        public void Clear()
        {
            m_Vertices.Clear();
            m_Triangles.Clear();
        }

        public int GetVertexCount()
        {
            return m_Vertices.Count;
        }

        public void StartNew(Mesh mesh)
        {
            mesh.Clear(false);
            Clear();
        }
    }
}
