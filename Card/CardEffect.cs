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

        public bool IsEnable { get; private set; } = false;

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


        public virtual void Enable()
        {
            IsEnable = true;
        }
        public virtual void Update() { }
        public virtual void Disable()
        {
            IsEnable = false;
        }

        public virtual void Reset() { }
    }
}
