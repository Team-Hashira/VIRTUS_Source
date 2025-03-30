using Hashira.CanvasUI;
using Hashira.Cards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Hashira
{
    public class CardSpreader : MonoBehaviour
    {
        [SerializeField] private SetupCardVisual _setupCardVisual;

        [Header("==========Spread setting==========")]
        [SerializeField] private int _horizontalCound;
        [SerializeField] private float _interval;
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
            int cardCount = _setupCardVisualList.Count;
            for (int i = 0; i < cardCount; i++)
            {
                float indexForCenter = i - (cardCount - 1) / 2f;
                float interval = _interval * Screen.width * Screen.height / (1080 * Screen.width);
                Vector3 targetPos = new Vector3(indexForCenter * interval + _offset.x, _offset.y, 0);
                targetPos = Quaternion.Euler(0, 0, indexForCenter * _angle) * targetPos;
                _setupCardVisualList[i].transform.position
                    = Vector3.Lerp(_setupCardVisualList[i].transform.position, transform.position + targetPos, Time.deltaTime * 10f);
            }
        }

        public void CardSpread(List<CardSO> cardList, bool isCurrent = false)
        {
            foreach (CardSO cardSO in cardList)
            {
                SetupCardVisual setupCardVisual = Instantiate(_setupCardVisual, transform);
                setupCardVisual.Setup(cardSO, isCurrent);
                setupCardVisual.transform.position = transform.position;
                _setupCardVisualList.Add(setupCardVisual);
            }
        }
    }
}
