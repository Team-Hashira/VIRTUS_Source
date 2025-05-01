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

        private bool _spreadEnable;

        public List<T> GetCardList<T>() where T : SetupCardVisual
        {
            return _setupCardVisualList.Select(cardVisual => cardVisual as T).ToList();
        }
        public List<SetupCardVisual> GetCardList()
        {
            return _setupCardVisualList;
        }

        private void Awake()
        {
            _setupCardVisualList = new List<SetupCardVisual>();
            _spreadEnable = true;
        }

        private void Update()
        {
            int cardCount = _setupCardVisualList.Count;
            for (int i = 0; i < cardCount; i++)
            {
                if (_spreadEnable)
                {
                    float xIndex;
                    float yIndex;
                    if (_horizontalCount == -1)
                    {
                        xIndex = (i - (cardCount - 1) * _pivot.x);
                        yIndex = 0;
                    }
                    else
                    {
                        xIndex = (i % _horizontalCount - (Mathf.Min(cardCount, _horizontalCount) - 1) * _pivot.x);
                        yIndex = (cardCount - 1) / _horizontalCount * _pivot.y - (i / _horizontalCount);
                    }

                    float xPos = xIndex * _interval.x + _offset.x;
                    float yPos = yIndex * _interval.y + _offset.y;
                    Vector2 targetPos = new Vector3(xPos, yPos);
                    targetPos = Quaternion.Euler(0, 0, xIndex * _angle) * targetPos;
                    _setupCardVisualList[i].RectTransform.anchoredPosition
                        = Vector3.Lerp(_setupCardVisualList[i].RectTransform.anchoredPosition, targetPos, 0.1f);
                }
                else
                {
                    _setupCardVisualList[i].RectTransform.anchoredPosition
                        = Vector3.Lerp(_setupCardVisualList[i].RectTransform.anchoredPosition, Vector2.zero, 0.1f);
                }
            }
        }

        public void SetSpreadEnable(bool enable)
        {
            _spreadEnable = enable;
        }

        public List<SetupCardVisual> CardSpread(List<CardSO> cardList)
        {
            foreach (CardSO cardSO in cardList)
            {
                SetupCardVisual setupCardVisual = Instantiate(_setupCardVisual, transform);
                setupCardVisual.VisualSetup(cardSO);
                setupCardVisual.transform.position = transform.position;
                setupCardVisual.RectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                setupCardVisual.RectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                _setupCardVisualList.Add(setupCardVisual);
            }
            return _setupCardVisualList;
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

        public void SetCardPosition()
        {
            int cardCount = _setupCardVisualList.Count;
            for (int i = 0; i < cardCount; i++)
            {
                float xIndex;
                float yIndex;
                if (_horizontalCount == -1)
                {
                    xIndex = (i - (cardCount - 1) / 2f);
                    yIndex = 0;
                }
                else
                {
                    xIndex = (i % _horizontalCount - (Mathf.Min(cardCount, _horizontalCount) - 1) / 2f);
                    yIndex = (float)((cardCount - 1) / _horizontalCount) / 2 - (i / _horizontalCount);
                }

                float xPos = xIndex * _interval.x + _offset.x;
                float yPos = yIndex * _interval.y + _offset.y;
                Vector2 targetPos = new Vector3(xPos, yPos);
                targetPos = Quaternion.Euler(0, 0, xIndex * _angle) * targetPos;
                _setupCardVisualList[i].RectTransform.anchoredPosition = targetPos;
            }
        }

        public void EnterCard(SetupCardVisual setupCardVisual, int index = -1)
        {
            if (index == -1)
                _setupCardVisualList.Add(setupCardVisual);
            else
                _setupCardVisualList.Insert(index, setupCardVisual);
        }
        public int ExitCard(SetupCardVisual setupCardVisual)
        {
            int index = _setupCardVisualList.IndexOf(setupCardVisual);
            _setupCardVisualList.Remove(setupCardVisual);
            return index;
        }
    }
}
