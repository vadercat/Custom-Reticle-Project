using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch.UI
{
    public class SliderWidget : MonoBehaviour, IWidget<int>
    {
        [SerializeField]
        private Slider m_Slider;

        [SerializeField]
        private InputField m_InputField;

        public System.Action<int> OnWidgetChanged { get; set; }

        private Object m_Changer;

        private void UpdateInputField()
        {
            m_InputField.text = m_Slider.value.ToString();
        }

        private void Start()
        {
            UpdateInputField();
        }

        public void OnSliderChanged(float value)
        {
            // If editor is slider.

            if (m_Changer == null)
            {
                m_Changer = m_Slider;
            }

            int intValue = (int)m_Slider.value;

            // Make changes to input field.

            if (m_Changer == m_Slider)
            {
                m_InputField.text = intValue.ToString();
                m_Changer = null;
            }

            if (OnWidgetChanged != null)
            {
                OnWidgetChanged.Invoke(intValue);
            }
        }

        public void OnInputFieldChanged(string value)
        {
            // If editor is input field.

            if (m_Changer == null)
            {
                m_Changer = m_InputField;
            }

            int intValue;

            int result;
            if (int.TryParse(value, out result))
            {
                intValue = result;
            }
            else
            {
                intValue = (int)m_Slider.value;
            }

            int minValue = (int)m_Slider.minValue;
            int maxValue = (int)m_Slider.maxValue;

            intValue = Mathf.Clamp(intValue, minValue, maxValue);

            // Make changes to slider.

            if (m_Changer == m_InputField)
            {
                m_Slider.value = intValue;
                m_Changer = null;
            }
        }

        public void OnInputFieldEndEdit(string value)
        {
            m_InputField.text = m_Slider.value.ToString();
        }

        public void SetInteractable(bool state)
        {
            m_Slider.interactable = state;
            m_InputField.interactable = state;
        }

        public int GetValue()
        {
            return (int)m_Slider.value;
        }
    }
}
