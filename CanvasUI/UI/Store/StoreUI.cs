using Hashira.Cards;
using UnityEngine;

namespace Hashira.CanvasUI.Stores
{
    public class StoreUI : UIBase, IToggleUI
    {
        [field: SerializeField] public string Key { get; set; }

        [SerializeField] private CardSpreader _cardSpreader;
        [SerializeField] private CardSetSO _allCardSO;



        protected override void Awake()
        {
            base.Awake();

            Close();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            _cardSpreader.CardSpread(_allCardSO.GetRandomCardList(8, CardManager.Instance.GetCardList()));
        }

        public void Close()
        {
            gameObject.SetActive(false);
            _cardSpreader.ClearCard();
        }

        private void OnDestroy()
        {
            UIManager.Instance?.RemoveUI(this);
        }
    }
}
