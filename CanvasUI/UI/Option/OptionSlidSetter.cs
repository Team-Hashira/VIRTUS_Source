using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI.Option
{
    public class OptionSlidSetter : OptionSetter<float>
    {
        [SerializeField] private Slider _slider;

        public override void Init()
        {
            _slider.onValueChanged.AddListener(value => OnValueEvent?.Invoke(value));
        }

        public override float Value
        {
            get =>_slider.value;
            set
            {
                _slider.value = value;
                OnValueEvent?.Invoke(value);
            }
        }
        public override Action<float> OnValueEvent { get; set; }

        public override void Active(bool active)
        {
            _slider.interactable = active;
        }
    }
}
