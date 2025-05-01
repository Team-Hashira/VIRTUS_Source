using Hashira.Cards;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Hashira.CanvasUI.Stores
{
    public class StoreUI : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private CardSpreader _cardSpreader, _addedCardSpreader;
        [SerializeField] private CardSetSO _allCardSO;
        [SerializeField] private CustomButton _exitButton, _buyBtn;
        [SerializeField] private CardDataArea _cardDataArea;
        [SerializeField] private TextMeshProUGUI _price, _itemCount, _cost;

        private SelecrableCard _currentSelectedCard;

        private List<SelecrableCard> _selectableCardList;

        protected override void Awake()
        {
            base.Awake();

            _selectableCardList = new List<SelecrableCard>();

            _exitButton.OnClickEvent += Close;
            _buyBtn.OnClickEvent += HandleBuyEvent;
            Close();
        }

        private void Start()
        {
            UpdatePrice();
        }

        private void Update()
        {
            _cost.text = $"{Cost.CurrentCost}";
        }

        private void HandleAddEvent(SelecrableCard selectableCard)
        {
            if (selectableCard != null)
            {
                _cardSpreader.ExitCard(selectableCard);
                selectableCard.transform.SetParent(_addedCardSpreader.transform);
                selectableCard.OnCardSelectEvent -= HandleAddEvent;
                selectableCard.OnCardSelectEvent += HandleRemoveEvent;
                _addedCardSpreader.EnterCard(selectableCard);

                selectableCard = null;
                _cardDataArea.gameObject.SetActive(false);
            }
            UpdatePrice();
        }

        private void HandleRemoveEvent(SelecrableCard selectableCard)
        {
            _addedCardSpreader.ExitCard(selectableCard);
            selectableCard.transform.SetParent(_cardSpreader.transform);
            selectableCard.OnCardSelectEvent -= HandleRemoveEvent;
            selectableCard.OnCardSelectEvent += HandleAddEvent;

            bool inserted = false;
            List<SetupCardVisual> cardList = _cardSpreader.GetCardList();
            for (int i = 0; i < cardList.Count; i++)
            {
                if ((cardList[i] as SelecrableCard).Index > selectableCard.Index)
                {
                    _cardSpreader.EnterCard(selectableCard, i);
                    inserted = true;
                    break;
                }
            }
            if (inserted == false)
                _cardSpreader.EnterCard(selectableCard);


            _currentSelectedCard = null;
            _cardDataArea.gameObject.SetActive(false);

            UpdatePrice();
        }

        private void UpdatePrice()
        {
            int cardCount = _addedCardSpreader.GetCardList().Count;
            _itemCount.text = $"{cardCount}의 상품";
            _price.text = $"{cardCount * 3}C";
            _price.color = Cost.CurrentCost < cardCount * 3 ? Color.red : Color.white;
        }

        private void HandleBuyEvent()
        {
            int cardCount = _addedCardSpreader.GetCardList().Count;
            if (Cost.TryRemoveCost(cardCount * 3))
            {
                foreach (SelecrableCard selecrableCard in _addedCardSpreader.GetCardList())
                {
                    CardManager.Instance.AddCard(selecrableCard.CardSO);

                    CardGetVisual cardGetVisual = Hashira.CanvasUI.UIManager.Instance.GetDomain<ToggleDomain>()
                        .OpenUI(nameof(CardGetVisual)) as CardGetVisual;
                    cardGetVisual.SetCard(selecrableCard.CardSO);
                }
                _addedCardSpreader.ClearCard();
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _cardSpreader.CardSpread(_allCardSO.GetRandomCardList(8, CardManager.Instance.GetCardList()));
            _cardSpreader.SetCardPosition();

            _cardDataArea.gameObject.SetActive(false);

            _selectableCardList = _cardSpreader.GetCardList<SelecrableCard>();
            int index = 0;
            _selectableCardList.ForEach(selectableCard =>
            {
                selectableCard.SetIndex(index);
                selectableCard.OnCardSelectEvent += HandleAddEvent;
                selectableCard.OnCardHoverEvent += HandleCardSelectEvent;

                index++;
            });


            UIManager.Instance.AddPauseMenu(this);
        }

        private void HandleCardSelectEvent(SelecrableCard card, bool isEnter)
        {
            if (isEnter)
            {
                _currentSelectedCard = card;
                _cardDataArea.gameObject.SetActive(true);
                _cardDataArea.SetCard(card.CardSO);
            }
            else if (_currentSelectedCard == card)
            {
                _currentSelectedCard = null;
                _cardDataArea.gameObject.SetActive(false);
            }
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _selectableCardList.ForEach(selectableCard =>
            {
                selectableCard.OnCardHoverEvent -= HandleCardSelectEvent;
                selectableCard.OnCardSelectEvent -= HandleAddEvent;
            });
            _cardSpreader.ClearCard();

            UIManager.Instance.RemovePauseMenu(this);
        }

        private void OnDestroy()
        {
            UIManager.Instance?.RemoveUI(this);
        }
    }
}
