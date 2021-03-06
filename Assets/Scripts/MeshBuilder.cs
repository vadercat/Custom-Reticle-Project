using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomReticle
{
    public static class MeshBuilder
    {
        public static void AppendOutlinedCircleCached(IMeshData meshData, List<Vector2> directions, float radius, float innerRadius, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            if (!forceAppend && (innerRadius >= radius || innerRadius <= 0.0f))
            {
                return;
            }

            int segments = directions.Count;

            if (segments < 3)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            int finalVertexCount = initialVertexCount + segments * 2;

            for (int i = 0; i < segments; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Vector2 point = directions[i] * (j == 0 ? radius : innerRadius);

                    // Vertices.

                    Vector2 newUV = new Vector2();

                    if (computeUVs)
                    {
                        // UVs.
                        newUV.x = Mathf.InverseLerp(-radius, radius, point.x);
                        newUV.y = Mathf.InverseLerp(-radius, radius, point.y);
                    }

                    meshData.AppendVertex(point, Vector3.back, newUV, color);
                }
            }

            // Triangles.

            AppendOutlinedCircleTriangles(meshData, initialVertexCount, finalVertexCount);
        }

        public static void AppendSolidCircleCached(IMeshData meshData, List<Vector2> directions, float radius, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            int segments = directions.Count;

            if (segments < 3)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            int finalVertexCount = initialVertexCount + segments;

            for (int i = 0; i < segments; i++)
            {
                Vector2 point = directions[i] * radius;

                // Vertices.

                Vector2 newUV = new Vector2();

                if (computeUVs)
                {
                    // UVs.
                    newUV.x = Mathf.InverseLerp(-radius, radius, point.x);
                    newUV.y = Mathf.InverseLerp(-radius, radius, point.y);
                }

                meshData.AppendVertex(point, Vector3.back, newUV, color);
            }

            // Triangles.

            AppendSolidCircleTriangles(meshData, initialVertexCount, finalVertexCount);
        }

        public static void AppendOutlinedCircle(IMeshData meshData, int segments, float radius, float innerRadius, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            if (!forceAppend && (innerRadius >= radius || innerRadius <= 0.0f))
            {
                return;
            }

            if (segments < 3)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            int finalVertexCount = initialVertexCount + segments * 2;

            for (int i = 0; i < segments; i++)
            {
                float radAngle = (float)i / segments * Mathf.PI * 2.0f;

                Vector2 direction = new Vector2()
                {
                    x = Mathf.Cos(radAngle),
                    y = Mathf.Sin(radAngle)
                };

                for (int j = 0; j < 2; j++)
                {
                    Vector2 point = direction * (j == 0 ? radius : innerRadius);

                    // Vertices.

                    Vector2 newUV = new Vector2();

                    if (computeUVs)
                    {
                        // UVs.
                        newUV.x = Mathf.InverseLerp(-radius, radius, point.x);
                        newUV.y = Mathf.InverseLerp(-radius, radius, point.y);
                    }

                    meshData.AppendVertex(point, Vector3.back, newUV, color);
                }
            }

            // Triangles.

            AppendOutlinedCircleTriangles(meshData, initialVertexCount, finalVertexCount);
        }

        public static void AppendSolidCircle(IMeshData meshData, int segments, float radius, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            if (segments < 3)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            int finalVertexCount = initialVertexCount + segments;

            for (int i = 0; i < segments; i++)
            {
                float radAngle = (float)i / segments * Mathf.PI * 2.0f;

                Vector2 point = new Vector2()
                {
                    x = Mathf.Cos(radAngle) * radius,
                    y = Mathf.Sin(radAngle) * radius
                };

                // Vertices.

                Vector2 newUV = new Vector2();

                if (computeUVs)
                {
                    // UVs.
                    newUV.x = Mathf.InverseLerp(-radius, radius, point.x);
                    newUV.y = Mathf.InverseLerp(-radius, radius, point.y);
                }

                meshData.AppendVertex(point, Vector3.back, newUV, color);
            }

            // Triangles.

            AppendSolidCircleTriangles(meshData, initialVertexCount, finalVertexCount);
        }

        public static void AppendOutlinedRectangle(IMeshData meshData, Vector2 position, Vector2 size, float thickness, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            Vector2 halfSize = size * 0.5f;

            Vector2 uvA = GetOutlinedRectangleVertexPosition(4, position, halfSize, thickness);
            Vector2 uvB = GetOutlinedRectangleVertexPosition(14, position, halfSize, thickness);

            for (int i = 0; i < 16; i++)
            {
                // Vertices.

                Vector2 vertPosition = GetOutlinedRectangleVertexPosition(i, position, halfSize, thickness);
                Vector2 newUV = new Vector2();

                if (computeUVs)
                {
                    // UVs.

                    newUV.x = Mathf.InverseLerp(uvA.x, uvB.x, vertPosition.x);
                    newUV.y = Mathf.InverseLerp(uvA.y, uvB.y, vertPosition.y);
                }

                meshData.AppendVertex(vertPosition, Vector3.back, newUV, color);
            }

            // Triangles.

            for (int i = 0; i < 4; i++)
            {
                int offset = initialVertexCount + i * 4;

                meshData.AppendTriangleVertexIndex(offset);
                meshData.AppendTriangleVertexIndex(offset + 1);
                meshData.AppendTriangleVertexIndex(offset + 2);

                meshData.AppendTriangleVertexIndex(offset + 2);
                meshData.AppendTriangleVertexIndex(offset + 3);
                meshData.AppendTriangleVertexIndex(offset);
            }
        }

        private static Vector2 GetOutlinedRectangleVertexPosition(int index, Vector2 position, Vector2 halfSize, float thickness)
        {
            switch (index)
            {
                // Right.
                case 0:
                    return position + new Vector2(halfSize.x, -halfSize.y);
                case 1:
                    return position + new Vector2(halfSize.x, halfSize.y);
                case 2:
                    return position + new Vector2(halfSize.x + thickness, halfSize.y);
                case 3:
                    return position + new Vector2(halfSize.x + thickness, -halfSize.y);
                // Bottom.
                case 4:
                    return position + new Vector2(-halfSize.x - thickness, -halfSize.y - thickness);
                case 5:
                    return position + new Vector2(-halfSize.x - thickness, -halfSize.y);
                case 6:
                    return position + new Vector2(halfSize.x + thickness, -halfSize.y);
                case 7:
                    return position + new Vector2(halfSize.x + thickness, -halfSize.y - thickness);
                // Left.
                case 8:
                    return position + new Vector2(-halfSize.x - thickness, -halfSize.y);
                case 9:
                    return position + new Vector2(-halfSize.x - thickness, halfSize.y);
                case 10:
                    return position + new Vector2(-halfSize.x, halfSize.y);
                case 11:
                    return position + new Vector2(-halfSize.x, -halfSize.y);
                // Top.
                case 12:
                    return position + new Vector2(-halfSize.x - thickness, halfSize.y);
                case 13:
                    return position + new Vector2(-halfSize.x - thickness, halfSize.y + thickness);
                case 14:
                    return position + new Vector2(halfSize.x + thickness, halfSize.y + thickness);
                case 15:
                    return position + new Vector2(halfSize.x + thickness, halfSize.y);
            }

            return Vector2.zero;
        }

        public static void AppendSolidRectangle(IMeshData meshData, Vector2 position, Vector2 size, Color color, bool computeUVs = false, bool forceAppend = false)
        {
            if (!forceAppend && color.a <= 0.0f)
            {
                return;
            }

            int initialVertexCount = meshData.GetVertexCount();
            Vector2 halfSize = size * 0.5f;

            Vector2 uvA = GetSolidRectangleVertexPosition(0, position, halfSize);
            Vector2 uvB = GetSolidRectangleVertexPosition(2, position, halfSize);

            for (int i = 0; i < 4; i++)
            {
                // Vertices.

                Vector2 vertPosition = GetSolidRectangleVertexPosition(i, position, halfSize);
                Vector2 newUV = new Vector2();

                if (computeUVs)
                {
                    // UVs.

                    newUV.x = Mathf.InverseLerp(uvA.x, uvB.x, vertPosition.x);
                    newUV.y = Mathf.InverseLerp(uvA.y, uvB.y, vertPosition.y);
                }

                meshData.AppendVertex(vertPosition, Vector3.back, newUV, color);
            }

            // Triangles.

            meshData.AppendTriangleVertexIndex(initialVertexCount);
            meshData.AppendTriangleVertexIndex(initialVertexCount + 1);
            meshData.AppendTriangleVertexIndex(initialVertexCount + 2);

            meshData.AppendTriangleVertexIndex(initialVertexCount + 2);
            meshData.AppendTriangleVertexIndex(initialVertexCount + 3);
            meshData.AppendTriangleVertexIndex(initialVertexCount);
        }

        private static Vector2 GetSolidRectangleVertexPosition(int index, Vector2 position, Vector2 halfSize)
        {
            switch (index)
            {
                case 0:
                    return position + new Vector2(-halfSize.x, -halfSize.y);
                case 1:
                    return position + new Vector2(-halfSize.x, halfSize.y);
                case 2:
                    return position + new Vector2(halfSize.x, halfSize.y);
                case 3:
                    return position + new Vector2(halfSize.x, -halfSize.y);
            }

            return Vector2.zero;
        }

        // Append triangles algorithms.

        private static void AppendOutlinedCircleTriangles(IMeshData meshData, int initialVertexCount, int finalVertexCount)
        {
            for (int i = initialVertexCount; i < finalVertexCount - 2; i += 2)
            {
                meshData.AppendTriangleVertexIndex(i);
                meshData.AppendTriangleVertexIndex(i + 1);
                meshData.AppendTriangleVertexIndex(i + 2);

                meshData.AppendTriangleVertexIndex(i + 2);
                meshData.AppendTriangleVertexIndex(i + 1);
                meshData.AppendTriangleVertexIndex(i + 3);
            }

            int last = finalVertexCount - 1;
            meshData.AppendTriangleVertexIndex(last - 1);
            meshData.AppendTriangleVertexIndex(last);
            meshData.AppendTriangleVertexIndex(initialVertexCount);

            meshData.AppendTriangleVertexIndex(initialVertexCount);
            meshData.AppendTriangleVertexIndex(last);
            meshData.AppendTriangleVertexIndex(initialVertexCount + 1);
        }

        private static void AppendSolidCircleTriangles(IMeshData meshData, int initialVertexCount, int finalVertexCount)
        {
            for (int i = initialVertexCount; i < finalVertexCount - 2; i++)
            {
                meshData.AppendTriangleVertexIndex(initialVertexCount);
                meshData.AppendTriangleVertexIndex(i + 2);
                meshData.AppendTriangleVertexIndex(i + 1);
            }
        }
    }
}
