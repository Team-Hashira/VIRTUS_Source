using Hashira.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira.CanvasUI
{
    public class CardScrollView : MonoBehaviour
    {
        [SerializeField] private SetupCardVisual _setupCardVisual;
        [SerializeField] private RectTransform _cardContent;

        private List<SetupCardVisual> _setupCardVisualList;

        private void Awake()
        {
            _setupCardVisualList = new List<SetupCardVisual>();
        }

        public List<T> GetCardList<T>() where T : SetupCardVisual
        {
            return _setupCardVisualList.Select(cardVisual => cardVisual as T).ToList();
        }


        public void CreateCard(List<CardSO> cardList, bool isCurrent = false)
        {
            foreach (CardSO cardSO in cardList)
            {
                SetupCardVisual setupCardVisual = Instantiate(_setupCardVisual, _cardContent);
                setupCardVisual.Setup(cardSO, isCurrent);
                setupCardVisual.transform.position = transform.position;
                _setupCardVisualList.Add(setupCardVisual);
            }
            _cardContent.sizeDelta = new Vector2(_cardContent.sizeDelta.x, 336.4f * ((_setupCardVisualList.Count+ - 1) / 8 + 1) - 92.5f);
        }
    }
}
