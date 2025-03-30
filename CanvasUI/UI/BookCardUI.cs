using Crogen.CrogenPooling;
using Hashira.Cards;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class BookCardUI : UIBase, IHoverableUI, IClickableUI, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField]
        private Image _cardImage;
        [SerializeField]
        private TextMeshProUGUI _descriptionText;

        private CardSO _cardSO;

        public OnClickEvent OnClickEvent;

        public void Initialize(CardSO cardSO)
        {
            _cardSO = cardSO;
            _cardImage.sprite = _cardSO.cardSprite;
            _descriptionText.text = PlayerDataManager.Instance.GetCardDescription(cardSO);
        }

        public void OnClick(bool isLeft)
        {
            if (isLeft)
            {
                OnClickEvent?.Invoke();
                CardBookInfoPanel panel = UIManager.Instance.GetDomain<ToggleDomain>().OpenUI("CardBookInfoPanel") as CardBookInfoPanel;
                panel.SetInfo(_cardSO);
            }
        }

        public void OnClickEnd(bool isLeft)
        {
        }

        public void OnCursorEnter()
        {
        }

        public void OnCursorExit()
        {
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
