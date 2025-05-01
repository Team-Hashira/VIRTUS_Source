using Hashira.CanvasUI;
using Hashira.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira.Cards
{
    public class CardManager : DontDestroyMonoSingleton<CardManager>
    {
        [field: SerializeField] public CardSetSO CardSetSO { get; private set; } 
        [SerializeField] private CardPackSO _defaultCardPackSO;

        public readonly int[] FixedCardNeedCost = { 2, 4, 6, 8 };
        public List<Pair<CardSO, int>> FixedCardList { get; private set; } = new List<Pair<CardSO, int>>();

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
            FixedCardList = new List<Pair<CardSO, int>>();
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
                FixedCardList.Add(new Pair<CardSO, int>(useableCardUI.CardSO, useableCardUI.Index));
                useableCardUI.SetFixationCard(true);
                OnFixationCardEvent?.Invoke(FixedCardList.Count);
            }
            else
            {
                PopupTextManager.Instance.PopupText("코스트가 부족합니다.", Color.red);
            }
        }

        public void UnFixationCard(UseableCardUI useableCardUI)
        {
            Pair<CardSO, int> RemovePair = null;
            for (int i = 0; i < FixedCardList.Count; i++)
            {
                if (FixedCardList[i].first == useableCardUI.CardSO)
                {
                    RemovePair = FixedCardList[i];
                }
            }
            FixedCardList.Remove(RemovePair);
            Cost.AddCost(FixedCardNeedCost[FixedCardList.Count]);
            useableCardUI.SetFixationCard(false);
            OnFixationCardEvent?.Invoke(FixedCardList.Count);
        }

        public List<CardSO> GetCardList() => _cardList;

        public List<CardSO> GetRandomCardList(int count)
        {
            CardSO[] results = new CardSO[count];

            FixedCardList.Select(pair => pair.first).ToList();

            for (int i = 0; i < FixedCardList.Count; i++)
            {
                results[FixedCardList[i].second] = FixedCardList[i].first;
            }
            FixedCardList.Clear();

            if (_cardList.Count > 0)
            {
                CardSO[] cards = _cardList.ToArray();
                int randomLastIndex = cards.Length - 1;

                for (int i = 0; i < count; i++)
                {
                    while (results[i] == null)
                    {
                        int randomIndex = Random.Range(0, randomLastIndex + 1);

                        // 해당 카드의 중첩 기대값
                        int currentCardCount =
                            PlayerDataManager.Instance.GetCardStack(cards[randomIndex]) +
                            CardCounting(results, cards[randomIndex]);

                        // 해당 카드가 MAX치를 찍을 수 있다면 그 카드는 제외
                        if (currentCardCount < cards[randomIndex].maxOverlapCount || cards[randomIndex].maxOverlapCount == -1)
                            results[i] = cards[randomIndex];
                        else
                        {
                            CardSO temp = cards[randomIndex];
                            cards[randomIndex] = cards[randomLastIndex];
                            cards[randomLastIndex] = temp;
                            randomLastIndex--;
                        }

                        if (randomLastIndex < 0) break;
                    }
                    if (randomLastIndex < 0) break;
                }
            }

            return results.Where(cardSO => cardSO != null).ToList();
        }

        public int CardCounting(CardSO[] cardSOs, CardSO targetCardSO)
        {
            int count = 0;
            foreach (CardSO cardSO in cardSOs)
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
