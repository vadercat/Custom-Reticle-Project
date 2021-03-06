using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace CustomReticle.Overwatch.UI
{
    public class DropdownWidget : MonoBehaviour, IWidget<int>
    {
        [SerializeField]
        private Dropdown m_Dropdown;

        public System.Action<int> OnWidgetChanged { get; set; }

        public void OnDropdownChanged(int value)
        {
            if (OnWidgetChanged != null)
            {
                OnWidgetChanged.Invoke(value);
            }
        }

        public void SetInteractable(bool state)
        {
            m_Dropdown.interactable = state;
        }

        public int GetValue()
        {
            return m_Dropdown.value;
        }
    }
}
