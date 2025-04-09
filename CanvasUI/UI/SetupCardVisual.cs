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
        private TextMeshProUGUI _cardNameText, _cardDescriptionText, _costText;

        public RectTransform _rectTransform;
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
        public CardSO CardSO { get; private set; }
        public Image CardBorderImage => _cardBorderImage;

        public void Setup(CardSO cardSO, bool isCurrent = false)
        {
            CardSO = cardSO;
            if (_cardBorderImage != null)
                _cardBorderImage.sprite = cardSO.cardborderSprite;
            if (_cardIconImage != null)
                _cardIconImage.sprite = cardSO.sprite;
            if (_cardNameText != null)
                _cardNameText.text = cardSO.displayName;
            if (_cardDescriptionText!= null)
                _cardDescriptionText.text = PlayerDataManager.Instance.GetCardDescription(cardSO);
            if (_costText != null)
            {
                bool isMaxStack = PlayerDataManager.Instance.GetCardStack(cardSO) == cardSO.maxOverlapCount;
                _costText.text = isMaxStack ? "M" : (cardSO.needCost + PlayerDataManager.Instance.GetAdditionalNeedCost(cardSO)).ToString();
            }
        }
    }
}
