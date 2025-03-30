using Hashira.Cards;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    [CreateAssetMenu(fileName = "CardPack", menuName = "SO/Card/CardPack")]
    public class CardPackSO : ScriptableObject
    {
        public List<CardSO> cardList;
    }
}
