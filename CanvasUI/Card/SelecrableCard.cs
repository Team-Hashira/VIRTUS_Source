using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class SelecrableCard : SetupCardVisual
    {
        [SerializeField] private CustomButton _customButton;

        public event Action<SelecrableCard> OnCardSelectEvent;

        private void Awake()
        {
            _customButton.OnClickEvent += () => OnCardSelectEvent?.Invoke(this);
        }

        public void ActiveHoverEvent(bool active) => _customButton.ActiveHoverEvent(active);
        public void ActiveClickEvent(bool active) => _customButton.ActiveClickEvent(active);
    }
}
