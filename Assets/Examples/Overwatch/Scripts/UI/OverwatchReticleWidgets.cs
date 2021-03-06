using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch.UI
{
    public class OverwatchReticleWidgets : MonoBehaviour
    {
        [SerializeField]
        private OverwatchReticle m_OverwatchReticle;

        [SerializeField]
        private Dropdown m_DropdownColors;

        [Header("Dropdown Widgets")]

        [SerializeField]
        private DropdownWidget m_TypeWidget;

        [SerializeField]
        private DropdownWidget m_ColorWidget;

        [Header("Toggle Widgets")]

        [SerializeField]
        private ToggleWidget m_ShowAccuracyWidget;

        [SerializeField]
        private ToggleWidget m_ScaleWithResolutionWidget;

        [Header("Slider Widgets")]

        [SerializeField]
        private SliderWidget m_ThicknessWidget;

        [SerializeField]
        private SliderWidget m_CrosshairLengthWidget;

        [SerializeField]
        private SliderWidget m_CenterGapWidget;

        [SerializeField]
        private SliderWidget m_OpacityWidget;

        [SerializeField]
        private SliderWidget m_OutlineOpacityWidget;

        [SerializeField]
        private SliderWidget m_DotSizeWidget;

        [SerializeField]
        private SliderWidget m_DotOpacityWidget;

        private void Start()
        {
            UpdateSpecialWidgets();

            // Fill dropdown colors.

            int colorsCount = OverwatchReticle.Colors.Length;
            List<string> options = new List<string>(colorsCount);
            for (int i = 0; i < colorsCount; i++)
            {
                Color32 color = OverwatchReticle.Colors[i];
                string hexColor = (color.a == 255) ? ColorUtility.ToHtmlStringRGB(color) : ColorUtility.ToHtmlStringRGBA(color);

                options.Add(string.Format("<color=#{1}>{0}</color>", "██████████", hexColor));
            }
            m_DropdownColors.ClearOptions();
            m_DropdownColors.AddOptions(options);

            // Dropdowns.

            m_TypeWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_Type = (ReticleType)value;
                OnWidgetChanged();
            };
            m_ColorWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_Color = value;
                OnWidgetChanged();
            };

            // Toggles.

            m_ShowAccuracyWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_ShowAccuracy = value;
                OnWidgetChanged();
            };
            m_ScaleWithResolutionWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_ScaleWithResolution = value;
                OnWidgetChanged();
            };

            // Sliders.

            m_ThicknessWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_Thickness = value;
                OnWidgetChanged();
            };
            m_CrosshairLengthWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_CrosshairLength = value;
                OnWidgetChanged();
            };
            m_CenterGapWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_CenterGap = value;
                OnWidgetChanged();
            };
            m_OpacityWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_Opacity = value;
                OnWidgetChanged();
            };
            m_OutlineOpacityWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_OutlineOpacity = value;
                OnWidgetChanged();
            };
            m_DotSizeWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_DotSize = value;
                OnWidgetChanged();
            };
            m_DotOpacityWidget.OnWidgetChanged += value =>
            {
                m_OverwatchReticle.m_DotOpacity = value;
                OnWidgetChanged();
            };
        }

        private void UpdateSpecialWidgets()
        {
            ReticleType reticleType = (ReticleType)m_TypeWidget.GetValue();
            m_CrosshairLengthWidget.SetInteractable(reticleType == ReticleType.Crosshairs || reticleType == ReticleType.CircleAndCrosshairs);

            m_CenterGapWidget.SetInteractable(!m_ShowAccuracyWidget.GetValue());
        }

        private void OnWidgetChanged()
        {
            m_OverwatchReticle.SetDitry();
            UpdateSpecialWidgets();
        }
    }
}
