using Hashira.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Hashira
{
    [CreateAssetMenu(fileName = "CardSetSO", menuName = "SO/CardSetSO")]
    public class CardSetSO : ScriptableObject
    {
        public List<CardSO> cardList;

        public List<CardSO> GetRandomCardList(int count, List<CardSO> exceptCardSOList = null)
        {
            List<CardSO> resultList = new List<CardSO>();

            if (cardList.Count == 0) return resultList;

            CardSO[] cards;
            if (exceptCardSOList != null)
                cards = cardList.Except(exceptCardSOList).ToArray();
            else
                cards = cardList.ToArray();

            int randomLastIndex = cards.Length - 1;
            for (int i = 0; resultList.Count < count; i++)
            {
                int randomIndex = Random.Range(0, randomLastIndex);

                resultList.Add(cards[randomIndex]);

                CardSO temp = cards[randomIndex];
                cards[randomIndex] = cards[randomLastIndex];
                cards[randomLastIndex] = temp;
                randomLastIndex--;

                if (randomLastIndex < 0) break;
            }
            return resultList;
        }
    }
}
