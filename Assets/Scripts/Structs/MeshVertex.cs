using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace CustomReticle
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MeshVertex
    {
        public Vector3 position;

        public Color32 color;

        public Vector2 uv;

        public MeshVertex(Vector3 position, Color32 color, Vector2 uv)
        {
            this.position = position;
            this.color = color;
            this.uv = uv;
        }
    }
}
