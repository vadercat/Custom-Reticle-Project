using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomReticle
{
    public interface IMeshData
    {
        void AppendVertex(Vector3 position, Vector3 vertexNormal, Vector2 uv, Color color);

        void AppendTriangleVertexIndex(int vertexIndex);

        int GetVertexCount();

        void Clear();

        void StartNew(Mesh mesh);

        void Apply(Mesh mesh);
    }
}
