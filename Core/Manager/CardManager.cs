using Hashira.CanvasUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Cards
{
    public class CardManager : DonDestroyMonoSingleton<CardManager>
    {
        [field: SerializeField] public CardSetSO CardSetSO { get; private set; } 
        [SerializeField] private CardPackSO _defaultCardPackSO;

        public readonly int[] FixedCardNeedCost = { 2, 4, 6, 8 };
        public List<CardSO> FixedCardList { get; private set; } = new List<CardSO>();

        private List<CardSO> _cardList = new List<CardSO>();

        public event Action<int> OnFixationCardEvent;

        public bool HasAllCard => CardSetSO.cardList.Count == _cardList.Count;

        protected override void Awake()
        {
            base.Awake();
            ClearCardList();
        }

        public void ClearCardList()
        {
            FixedCardList = new List<CardSO>();
            _cardList = new List<CardSO>();
            foreach (CardSO cardSO in _defaultCardPackSO.cardList)
            {
                AddCard(cardSO);
            }
        }

        private void Update()
        {
            // 코스트 증가 디버그
//#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.C))
            {
                Cost.AddCost(100);
            }
//#endif
        }

        public void AddCard(CardSO cardSO)
        {
            _cardList.Add(cardSO);
        }
        public void RemoveCard(CardSO cardSO)
        {
            _cardList.Remove(cardSO);
        }
        //웬만하면 이거 사용
        public void RemoveCard(int index)
        {
            _cardList.RemoveAt(index);
        }

        public void FixationCard(UseableCardUI useableCardUI, bool isUseCost = true)
        {
            if (FixedCardNeedCost.Length == FixedCardList.Count) return;
            if (isUseCost == false || Cost.TryRemoveCost(FixedCardNeedCost[FixedCardList.Count]))
            {
                FixedCardList.Add(useableCardUI.CardSO);
                useableCardUI.SetFixationCard(true);
                OnFixationCardEvent?.Invoke(FixedCardList.Count);
            }
        }

        public void UnFixationCard(UseableCardUI useableCardUI)
        {
            FixedCardList.Remove(useableCardUI.CardSO);
            Cost.AddCost(FixedCardNeedCost[FixedCardList.Count]);
            useableCardUI.SetFixationCard(false);
            OnFixationCardEvent?.Invoke(FixedCardList.Count);
        }

        public List<CardSO> GetCardList() => _cardList;

        public List<CardSO> GetRandomCardList(int count)
        {
            List<CardSO> resultList = FixedCardList.ToList();
            FixedCardList.Clear();

            if (_cardList.Count == 0) return resultList;

            CardSO[] cards = _cardList.ToArray();
            int randomLastIndex = cards.Length - 1;
            for (int i = 0; resultList.Count < count; i++)
            {
                int randomIndex = Random.Range(0, randomLastIndex + 1);

                // 해당 카드의 중첩 기대값
                int currentCardCount =
                    PlayerDataManager.Instance.GetCardStack(cards[randomIndex]) +
                    CardCounting(resultList, cards[randomIndex]);

                // 해당 카드가 MAX치를 찍을 수 있다면 그 카드는 제외
                if (currentCardCount < cards[randomIndex].maxOverlapCount || cards[randomIndex].maxOverlapCount == -1)
                    resultList.Add(cards[randomIndex]);
                else
                {
                    CardSO temp = cards[randomIndex];
                    cards[randomIndex] = cards[randomLastIndex];
                    cards[randomLastIndex] = temp;
                    randomLastIndex--;
                }

                if (randomLastIndex < 0) break;
            }
            return resultList;
        }

        public int CardCounting(List<CardSO> cardSOList, CardSO targetCardSO)
        {
            int count = 0;
            foreach (CardSO cardSO in cardSOList)
            {
                if (cardSO == targetCardSO)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
