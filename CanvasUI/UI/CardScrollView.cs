using Hashira.Cards;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.CanvasUI
{
    public class CardScrollView : MonoBehaviour
    {
        [SerializeField] private SetupCardVisual _setupCardVisual;
        [SerializeField] private RectTransform _cardContent;
        private GridLayoutGroup _gridLayoutGroup;
        private float _startYSize;

        private List<SetupCardVisual> _setupCardVisualList;

        private void Awake()
        {
            _gridLayoutGroup = _cardContent.GetComponent<GridLayoutGroup>();
            _setupCardVisualList = new List<SetupCardVisual>();
            _startYSize = _cardContent.sizeDelta.y;
        }

        public void ActiveGrid(bool active)
        {
            _gridLayoutGroup.enabled = active;
        }

        public List<T> GetCardList<T>() where T : SetupCardVisual
        {
            return _setupCardVisualList.Select(cardVisual => cardVisual as T).ToList();
        }
        public List<SetupCardVisual> GetCardList()
        {
            return _setupCardVisualList;
        }

        public void CreateCard(List<CardSO> cardList, bool isCurrent = false, bool isExceptMaxStack = false)
        {
            foreach (CardSO cardSO in cardList)
            {
                if (isExceptMaxStack && PlayerDataManager.Instance.IsMaxStackEffect(cardSO))
                    continue;
                SetupCardVisual setupCardVisual = Instantiate(_setupCardVisual, _cardContent);
                setupCardVisual.VisualSetup(cardSO, isCurrent);
                setupCardVisual.transform.position = transform.position;
                _setupCardVisualList.Add(setupCardVisual);
            }

            StartCoroutine(DelaySizeSetting());
        }

        private IEnumerator DelaySizeSetting()
        {
            yield return new WaitForEndOfFrame();

            if (_setupCardVisualList.Count > 0)
            {
                _cardContent.sizeDelta = new Vector2(_cardContent.sizeDelta.x, _startYSize + (_setupCardVisualList[0].transform.position.y - _setupCardVisualList[^1].transform.position.y));
            }
        }

        public void ClearCard()
        {
            foreach (var cardVisual in _setupCardVisualList)
            {
                if (cardVisual != null && cardVisual.transform.parent == _cardContent.transform)
                    Destroy(cardVisual.gameObject);
            }
            _setupCardVisualList.Clear();
        }
    }
}
