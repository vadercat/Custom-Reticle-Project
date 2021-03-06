using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomReticle
{
    [RequireComponent(typeof(MeshFilter))]
    public class Reticle : MonoBehaviour
    {
        private Mesh m_Mesh;

        private IMeshData m_MeshData = new MeshDataSimple(); // Any other IMeshData implementation.

        public void ApplyMeshData()
        {
            m_MeshData.Apply(m_Mesh);
        }

        public IMeshData GetNewMeshData()
        {
            EnsureMeshInstance();

            m_MeshData.StartNew(m_Mesh);
            return m_MeshData;
        }

        private void EnsureMeshInstance()
        {
            if (m_Mesh != null)
            {
                return;
            }

            m_Mesh = new Mesh();
            m_Mesh.name = "Custom Reticle";
            m_Mesh.hideFlags = HideFlags.HideAndDontSave;
        }

        private void Awake()
        {
            EnsureMeshInstance();

            MeshFilter meshFilter = GetComponent<MeshFilter>();
            meshFilter.sharedMesh = m_Mesh;
        }

        private void OnDestroy()
        {
            Destroy(m_Mesh);
        }
    }
}
