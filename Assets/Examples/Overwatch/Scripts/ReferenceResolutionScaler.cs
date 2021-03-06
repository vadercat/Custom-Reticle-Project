using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch
{
    public class ReferenceResolutionScaler : MonoBehaviour
    {
        [SerializeField]
        private Camera m_Camera;

        [SerializeField]
        private CanvasScaler m_CanvasScaler;

        [SerializeField]
        private Vector2Int m_ReferenceResolution = new Vector2Int(1920, 1080);

        public Vector2Int GetReferenceResolution()
        {
            return m_ReferenceResolution;
        }

        private void Awake()
        {
            if (m_Camera != null)
            {
                m_Camera.orthographicSize = m_ReferenceResolution.y * 0.5f;
            }
            else
            {
                Debug.LogWarning("Camera is not assigned.");
            }

            if (m_CanvasScaler != null)
            {
                m_CanvasScaler.referenceResolution = m_ReferenceResolution;
            }
            else
            {
                Debug.LogWarning("Canvas Scaler is not assigned.");
            }
        }
    }
}
