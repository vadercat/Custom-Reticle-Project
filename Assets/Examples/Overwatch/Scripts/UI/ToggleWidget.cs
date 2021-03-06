using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch.UI
{
    public class ToggleWidget : MonoBehaviour, IWidget<bool>
    {
        [SerializeField]
        private Toggle m_Toggle;

        public System.Action<bool> OnWidgetChanged { get; set; }

        public void OnToggleChanged(bool value)
        {
            if (OnWidgetChanged != null)
            {
                OnWidgetChanged.Invoke(value);
            }
        }

        public void SetInteractable(bool state)
        {
            m_Toggle.interactable = state;
        }

        public bool GetValue()
        {
            return m_Toggle.isOn;
        }
    }
}
