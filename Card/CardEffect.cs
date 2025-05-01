using Hashira.Items;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public abstract class CardEffect : ItemEffect
    {
        public CardSO CardSO { get; private set; }
        [HideInInspector] public int stack;

        [HideInInspector] public Player player;

        public bool IsMaxStack => stack == CardSO.maxOverlapCount;

        public override void Initialize(ItemSO itemSO)
        {
            base.Initialize(itemSO);
            CardSO = itemSO as CardSO;
            stack = 0;
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
