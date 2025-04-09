using Hashira.Items;
using Hashira.Players;
using System;
using System.Collections.Generic;

namespace Hashira.Cards.Effects
{
    public abstract class CardEffect : ItemEffect<CardEffect>
    {
        public CardSO CardSO { get; private set; }
        public int stack;

        protected abstract int[] _NeedCostByStack { get; }

        public Player player;

        public bool IsMaxStack => stack == CardSO.maxOverlapCount;

        public override void Initialize(ItemSO<CardEffect> itemSO)
        {
            base.Initialize(itemSO);
            CardSO = itemSO as CardSO;
            stack = 1;
        }

        public int GetAdditionalNeedCost()
        {
            int needCost = 0;
            for (int i = 0; i < stack; i++)
            {
                if (i < _NeedCostByStack.Length)
                    needCost += _NeedCostByStack[i];
                else
                    needCost += _NeedCostByStack[^1];
            }
            return needCost;
        }

        public virtual string GetCardDescription()
        {
            if (stack < CardSO.cardDescriptions.Length)
            {
                return CardSO.cardDescriptions[stack];
            }
            else
            {
                return CardSO.cardDescriptions[^1];
            }
        }


        public abstract void Enable();
        public abstract void Update();
        public abstract void Disable();
    }
}
