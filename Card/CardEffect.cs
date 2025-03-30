using Hashira.Players;
using System;
using System.Collections.Generic;

namespace Hashira.Cards.Effects
{
    public abstract class CardEffect
    {
        public CardSO CardSO { get; private set; }
        public int stack;

        protected abstract int[] _NeedCostByStack { get; }

        public Player player;

        public void Init(CardSO cardSO)
        {
            CardSO = cardSO;
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
