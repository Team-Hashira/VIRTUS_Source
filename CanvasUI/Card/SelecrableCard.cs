using Hashira.Cards;
using System;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class SelecrableCard : SetupCardVisual
    {
        [SerializeField] private CustomButton _customButton;

        public event Action<SelecrableCard> OnCardSelectEvent;
        public event Action<SelecrableCard, bool> OnCardHoverEvent;

        public int Index { get; private set; }

        private void Awake()
        {
            _customButton.OnClickEvent += () => OnCardSelectEvent?.Invoke(this);
            _customButton.OnHoverEvent += isEnter => OnCardHoverEvent?.Invoke(this, isEnter);
        }

        public void SetIndex(int index)
        {
            Index = index;
        }

        public void ActiveHoverEvent(bool active) => _customButton.ActiveHoverEvent(active);
        public void ActiveClickEvent(bool active) => _customButton.ActiveClickEvent(active);
    }
}
