using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomReticle.Overwatch.UI
{
    public interface IWidget<T>
    {
        System.Action<T> OnWidgetChanged { get; set; }

        void SetInteractable(bool state);

        T GetValue();
    }
}
