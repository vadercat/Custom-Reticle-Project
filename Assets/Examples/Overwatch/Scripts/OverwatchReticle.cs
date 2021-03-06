using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch
{
    public class OverwatchReticle : MonoBehaviour
    {
        [Header("References")]

        [SerializeField]
        private Reticle m_Reticle;

        [SerializeField]
        private ReferenceResolutionScaler m_ReferenceResolutionScaler;

        [Header("Properties")]

        [SerializeField]
        private bool m_UpdateEveryFrame = false;

        [SerializeField]
        private float m_CircleQuality = 15.625f;

        [SerializeField]
        private float m_DotQuality = 62.5f;

        [SerializeField]
        private Color m_OutlineColor = Color.black;

        [SerializeField]
        private float m_OutlineThickness = 1.0f;

        [SerializeField]
        public static readonly Color32[] Colors = new Color32[]
        {
            new Color32(255, 255, 255, 255),
            new Color32(0, 255, 0, 255),
            new Color32(0, 255, 255, 255),
            new Color32(255, 0, 0, 255),
            new Color32(255, 0, 255, 255),
            new Color32(255, 255, 0, 255),
            new Color32(0, 0, 0, 255),
            new Color32(255, 112, 0, 255),
            new Color32(254, 108, 183, 255),
            new Color32(22, 55, 236, 255),
            new Color32(148, 12, 255, 255),
            new Color32(249, 251, 156, 255),
            new Color32(1, 136, 29, 255)
        };

        [Header("Reticle Options")]

        public ReticleType m_Type = ReticleType.Circle;

        public bool m_ShowAccuracy = true;

        public int m_Color = 0;

        [Range(1, 15)]
        public int m_Thickness = 1;

        [Range(0, 50)]
        public int m_CrosshairLength = 25;

        [Range(0, 100)]
        public int m_CenterGap = 30;

        [Range(0, 100)]
        public int m_Opacity = 80;

        [Range(0, 100)]
        public int m_OutlineOpacity = 50;

        [Range(2, 10)]
        public int m_DotSize = 6;

        [Range(0, 100)]
        public int m_DotOpacity = 100;

        public bool m_ScaleWithResolution = true;

        private List<Vector2> m_SegmentsDirections = new List<Vector2>(512);

        private bool m_IsDirty = true;

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_Color = Mathf.Clamp(m_Color, 0, Colors.Length - 1);
        }
#endif

        public void SetDitry()
        {
            m_IsDirty = true;
        }

        private void Start()
        {
            UpdateReticle();
        }

        private void LateUpdate()
        {
            UpdateReticle();
        }

        private void UpdateReticle()
        {
            if (m_Reticle != null && (m_IsDirty || m_UpdateEveryFrame))
            {
                m_IsDirty = false;

                // Scale.

                float scale = 1.0f;

                if (!m_ScaleWithResolution)
                {
                    scale = (float)m_ReferenceResolutionScaler.GetReferenceResolution().y / Screen.height;
                }

                m_Reticle.transform.localScale = new Vector3(scale, scale, scale);

                // Mesh preparing.

                IMeshData meshData = m_Reticle.GetNewMeshData();

                #region Colors

                Color mainColor = Colors[m_Color];

                float alpha = (float)m_Opacity / 100.0f;
                float dotAlpha = (float)m_DotOpacity / 100.0f;
                float outlineAlpha = (float)m_OutlineOpacity / 100.0f;

                // Circle color.

                Color circleColor = mainColor;
                circleColor.a = alpha;

                Color circleOutlineColor = m_OutlineColor;
                circleOutlineColor.a = alpha * outlineAlpha * m_OutlineColor.a;

                // Crosshairs color.

                float crosshairsAlphaMul = Mathf.Clamp01(Mathf.Pow(alpha, 1.0f));

                Color crosshairsColor = mainColor;
                crosshairsColor.a = alpha * crosshairsAlphaMul;

                Color crosshairsOutlineColor = m_OutlineColor;
                crosshairsOutlineColor.a = alpha * outlineAlpha * crosshairsAlphaMul * m_OutlineColor.a;

                // Dot color.

                Color dotColor = mainColor;
                dotColor.a = dotAlpha;

                Color dotOutlineColor = m_OutlineColor;
                dotOutlineColor.a = dotAlpha * outlineAlpha * m_OutlineColor.a;

                #endregion

                // Circle geometry.

                if (m_Type == ReticleType.Circle || m_Type == ReticleType.CircleAndCrosshairs)
                {
                    float circleRadius;

                    if (m_ShowAccuracy)
                    {
                        float lerp = Mathf.Sin(GetTime() * Mathf.PI) * 0.5f + 0.5f;
                        circleRadius = Mathf.Lerp(30.0f, 100.0f, lerp) * 0.5f;
                        SetDitry();
                    }
                    else
                    {
                        circleRadius = m_CenterGap * 0.5f;
                    }

                    if (circleRadius > 0.0f)
                    {
                        float circleInnerRadius = Mathf.Max(circleRadius - m_Thickness * 2, 1.0f);
                        int circleSegments = Mathf.FloorToInt(Mathf.PI * 2.0f * circleRadius * (m_CircleQuality / 100.0f));

                        SetCircleSegments(circleSegments);

                        MeshBuilder.AppendOutlinedCircleCached(meshData, m_SegmentsDirections, circleRadius, circleInnerRadius, circleColor);
                        MeshBuilder.AppendOutlinedCircleCached(meshData, m_SegmentsDirections, circleRadius + m_OutlineThickness, circleRadius, circleOutlineColor);
                        MeshBuilder.AppendOutlinedCircleCached(meshData, m_SegmentsDirections, circleInnerRadius, circleInnerRadius - m_OutlineThickness, circleOutlineColor);
                    }
                }

                // Crosshairs geometry.

                if (m_CrosshairLength > 0 && (m_Type == ReticleType.Crosshairs || m_Type == ReticleType.CircleAndCrosshairs))
                {
                    Vector2 crosshairPosition;
                    Vector2 crosshairSize;

                    float crosshairsRadius;

                    if (m_ShowAccuracy)
                    {
                        float minRadius = (m_Type == ReticleType.CircleAndCrosshairs) ? 30.0f : 1.5f;

                        float lerp = Mathf.Sin(GetTime() * Mathf.PI) * 0.5f + 0.5f;
                        crosshairsRadius = Mathf.Lerp(minRadius, 100.0f, lerp) * 0.5f;
                        SetDitry();
                    }
                    else
                    {
                        crosshairsRadius = m_CenterGap * 0.5f;
                    }

                    int crosshairThickness = m_Thickness * 2;

                    // Right rectange.

                    crosshairPosition = new Vector2(crosshairsRadius + m_CrosshairLength * 0.5f, 0.0f);
                    crosshairSize = new Vector2(m_CrosshairLength, crosshairThickness);

                    MeshBuilder.AppendSolidRectangle(meshData, crosshairPosition, crosshairSize, crosshairsColor);
                    MeshBuilder.AppendOutlinedRectangle(meshData, crosshairPosition, crosshairSize, m_OutlineThickness, crosshairsOutlineColor);

                    // Bottom rectange.

                    crosshairPosition = new Vector2(0.0f, -crosshairsRadius - m_CrosshairLength * 0.5f);
                    crosshairSize = new Vector2(crosshairThickness, m_CrosshairLength);

                    MeshBuilder.AppendSolidRectangle(meshData, crosshairPosition, crosshairSize, crosshairsColor);
                    MeshBuilder.AppendOutlinedRectangle(meshData, crosshairPosition, crosshairSize, m_OutlineThickness, crosshairsOutlineColor);

                    // Left rectange.

                    crosshairPosition = new Vector2(-crosshairsRadius - m_CrosshairLength * 0.5f, 0.0f);
                    crosshairSize = new Vector2(m_CrosshairLength, crosshairThickness);

                    MeshBuilder.AppendSolidRectangle(meshData, crosshairPosition, crosshairSize, crosshairsColor);
                    MeshBuilder.AppendOutlinedRectangle(meshData, crosshairPosition, crosshairSize, m_OutlineThickness, crosshairsOutlineColor);

                    // Top rectange.

                    crosshairPosition = new Vector2(0.0f, crosshairsRadius + m_CrosshairLength * 0.5f);
                    crosshairSize = new Vector2(crosshairThickness, m_CrosshairLength);

                    MeshBuilder.AppendSolidRectangle(meshData, crosshairPosition, crosshairSize, crosshairsColor);
                    MeshBuilder.AppendOutlinedRectangle(meshData, crosshairPosition, crosshairSize, m_OutlineThickness, crosshairsOutlineColor);
                }

                // Dot geometry.

                float dotRadius = m_DotSize * 0.5f;

                if (dotRadius > 0.0f)
                {
                    int dotSegments = Mathf.FloorToInt(Mathf.PI * 2.0f * dotRadius * (m_DotQuality / 100.0f));

                    SetCircleSegments(dotSegments);

                    MeshBuilder.AppendSolidCircleCached(meshData, m_SegmentsDirections, dotRadius, dotColor);
                    MeshBuilder.AppendOutlinedCircleCached(meshData, m_SegmentsDirections, dotRadius + m_OutlineThickness, dotRadius, dotOutlineColor);
                }

                // Apply and bake reticle mesh.

                m_Reticle.ApplyMeshData();
            }
        }

        private void SetCircleSegments(int segments)
        {
            m_SegmentsDirections.Clear();

            for (int i = 0; i < segments; i++)
            {
                float radAngle = (float)i / segments * Mathf.PI * 2.0f;

                Vector2 direction = new Vector2()
                {
                    x = Mathf.Cos(radAngle),
                    y = Mathf.Sin(radAngle)
                };

                m_SegmentsDirections.Add(direction);
            }
        }

        private float GetTime()
        {
            return Time.unscaledTime;
        }
    }
}
