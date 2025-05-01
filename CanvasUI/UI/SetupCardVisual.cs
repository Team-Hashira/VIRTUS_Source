using Hashira.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class SetupCardVisual : MonoBehaviour
    {
        [SerializeField]
        private Image _cardBorderImage, _cardIconImage;
        [SerializeField]
        protected TextMeshProUGUI _cardNameText, _cardDescriptionText, _costText;
        [field: SerializeField]
        public ChildrenMaterialController MaterialController { get; protected set; }
        [field: SerializeField]
        public CanvasGroup CanvasGroup { get; protected set; }

        private RectTransform _rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = transform as RectTransform;
                }
                return _rectTransform;
            }
        }
        public CardSO CardSO { get; protected set; }
        public Image CardBorderImage => _cardBorderImage;

        public virtual void VisualSetup(CardSO cardSO)
        {
            CardSO = cardSO;
            if (_cardBorderImage != null)
                _cardBorderImage.color = CardColor.GetColorByType(cardSO.cardType);
            if (_cardIconImage != null)
                _cardIconImage.sprite = cardSO.sprite;
            if (_cardNameText != null)
                _cardNameText.text = cardSO.displayName;
            if (_cardDescriptionText!= null)
                _cardDescriptionText.text = PlayerDataManager.Instance.GetCardDescription(cardSO);
            if (_costText != null)
            {
                bool isMaxStack = PlayerDataManager.Instance.GetCardStack(cardSO) == cardSO.maxOverlapCount;
                _costText.text = isMaxStack ? "M" : PlayerDataManager.Instance.GetCardNeedCost(CardSO).ToString();
            }
        }
    }
}
