using System;
using UnityEngine;

namespace Hashira.CanvasUI.Option
{
    public abstract class OptionSetter<T> : OptionSetterBase
    {
        public abstract T Value { get; set; }
        public abstract Action<T> OnValueEvent { get; set; }
    }

    public abstract class OptionSetterBase : MonoBehaviour
    {
        public string setterTarget;
        public abstract void Init();
        public abstract void Active(bool active);
    }
}
