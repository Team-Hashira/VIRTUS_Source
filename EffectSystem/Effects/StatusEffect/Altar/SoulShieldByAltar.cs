using Crogen.CrogenPooling;
using Hashira.Core.DamageHandler;
using Hashira.VFX;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class SoulShieldByAltar : AltarEffect
    {
        protected override EffectPoolType EffectVFX { get; } = EffectPoolType.SoulShieldVFX;
        public override int MaxCount { get; set; } = 3;

        public override void Enable()
        {
            base.Enable();
            var shieldHandler = new ShieldHandler(3, true);
            _health.AddDamageHandler(EDamageHandlerLayer.First, shieldHandler);
            Debug.Log(_effectVFXObject.gameObject.transform);
            SetBrokenLevel();
        }

        protected override void HandleHit(int hp)
        {
            base.HandleHit(hp);
            SetBrokenLevel();
        }

        private void SetBrokenLevel() => (_effectVFXObject as SoulShieldVFX)?.SetBrokenLevel(MaxCount-Count, MaxCount);
    }
}