using AYellowpaper.SerializedCollections;
using Hashira.Cards.Effects;
using Hashira.EffectSystem;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards
{
    public enum ECardType
    {
        Bullet,
        Stat,
        Magic
    }

    [CreateAssetMenu(fileName = "Card", menuName = "SO/Card/Card")]
    public class CardSO : ItemSO<CardEffect>
    {
        [Header("==========CardSO==========")]
        public ECardType cardType;
        public Sprite cardborderSprite;
        [TextArea]
        public string[] cardDescriptions;
        public int needCost;
        public int maxOverlapCount;

        public override string Description { get => PlayerDataManager.Instance.GetCardDescription(this); }
    }
}
