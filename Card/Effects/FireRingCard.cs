using Crogen.CrogenPooling;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class FireRingCard : MagicCardEffect
    {
        private int[] _needCostByStack = { 1, 1, 3 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private float[] _delayByStack = { 18f, 15f, 12f, 9f };
        protected override float DelayTime => _delayByStack[stack - 1];

        private float _durationByStack = 5f;
        private int[] _tickDamageByStack = { 4, 6, 8, 10 };
        private int[] _endBurstDamageByStack = { 50, 50, 50, 50 };

        public override void Use()
        {
            FireRing fireRing = PopCore.Pop(CardSubPoolType.FireRing, player.transform) as FireRing;
            fireRing.Init(_tickDamageByStack[stack - 1], _endBurstDamageByStack[stack - 1], _durationByStack);
        }

        public override void SetMultiplier(float multiplier)
        {
            base.SetMultiplier(multiplier);
            _delayMultiplier = Mathf.Clamp(_delayMultiplier, 0f, 1f);
        }
    }
}
