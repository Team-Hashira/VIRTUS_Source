using DG.Tweening;
using Hashira.Cards;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class CardBookInfoPanel : UIBase, IToggleUI
    {
        [field: SerializeField]
        public string Key { get; set; }

        public event OnToggleEvent OnToggleEvent;

        private Image _noImage;
        private TextMeshProUGUI _tmp;

        private float _xSize = Screen.width * 0.37f;

        protected override void Awake()
        {
            base.Awake();
            RectTransform.sizeDelta = new Vector2(_xSize, RectTransform.sizeDelta.y);
            RectTransform.anchoredPosition = new Vector2(_xSize, RectTransform.anchoredPosition.y);
        }

        public void SetInfo(CardSO card)
        {

        }

        public void Open()
        {
            OnToggleEvent?.Invoke(true);
            RectTransform.DOAnchorPosX(0, 0.4f);
        }

        public void Close()
        {
            OnToggleEvent?.Invoke(false);
            RectTransform.DOAnchorPosX(_xSize, 0.4f);
        }
    }
}
