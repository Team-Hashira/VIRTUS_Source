using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public abstract class AltarEffect : Effect, ICountingEffect, ISingleEffect
    {
        protected abstract EffectPoolType EffectVFX { get; }
        protected IPoolingObject _effectVFXObject;
        
        protected EntityHealth _health;
        
        public abstract int MaxCount { get; set; }
        public int Count { get; set; }

        public Action<Effect> OnAddEffectEvent { get; set; }
        
        protected virtual void HandleHit(int hp)
        {
            Count++;
        }

        public override void Enable()
        {
            base.Enable();
            _health = entity.GetEntityComponent<EntityHealth>(); 
            _health.OnHitEvent += HandleHit;
            _effectVFXObject = PopCore.Pop(EffectVFX, entity.transform);
        }

        public override void Disable()
        {
            _effectVFXObject.Push();
            _health.OnHitEvent -= HandleHit;
            base.Disable();
        }


        public void OnAddEffect(Effect effect)
        {
            Count = 0;
        }
    }
}