using Hashira.CanvasUI;
using Hashira.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class CardSpreader : MonoBehaviour
    {
        [SerializeField] private SetupCardVisual _setupCardVisual;

        [Header("==========Spread setting==========")]
        [SerializeField] private int _horizontalCount = -1;
        [SerializeField] private Vector2 _pivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 _interval = new Vector2(200, 0);
        [SerializeField] private float _angle;
        [SerializeField] private Vector2 _offset;
        [Header("==========Test==========")]

        private List<SetupCardVisual> _setupCardVisualList;

        public List<T> GetCardList<T>() where T : SetupCardVisual
        {
            return _setupCardVisualList.Select(cardVisual => cardVisual as T).ToList();
        }

        private void Awake()
        {
            _setupCardVisualList = new List<SetupCardVisual>();
        }

        private void Update()
        {
            int horizontalCount = _horizontalCount == -1 ? int.MaxValue : _horizontalCount;
            int cardCount = Mathf.Min(_setupCardVisualList.Count, horizontalCount);
            for (int i = 0; i < cardCount; i++)
            {
                float indexForCenter = (i - (cardCount - 1) / 2f) % horizontalCount;
                float xPos = indexForCenter * _interval.x + _offset.x;
                float yPos = -(i / horizontalCount) * _interval.y + _offset.y;
                Vector2 targetPos = new Vector3(xPos, yPos);
                targetPos = Quaternion.Euler(0, 0, indexForCenter * _angle) * targetPos;
                _setupCardVisualList[i].RectTransform.anchoredPosition
                    = Vector3.Lerp(_setupCardVisualList[i].RectTransform.anchoredPosition, targetPos, Time.deltaTime * 10f);
            }
        }

        public void CardSpread(List<CardSO> cardList, bool isCurrent = false)
        {
            foreach (CardSO cardSO in cardList)
            {
                SetupCardVisual setupCardVisual = Instantiate(_setupCardVisual, transform);
                setupCardVisual.Setup(cardSO, isCurrent);
                setupCardVisual.transform.position = transform.position;
                setupCardVisual.RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                setupCardVisual.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _setupCardVisualList.Add(setupCardVisual);
            }
        }

        public void ClearCard()
        {
            if (_setupCardVisualList == null) return;
            foreach (var cardVisual in _setupCardVisualList)
            {
                Destroy(cardVisual.gameObject);
            }
            _setupCardVisualList.Clear();
        }
    }
}
