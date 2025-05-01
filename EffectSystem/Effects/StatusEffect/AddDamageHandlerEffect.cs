using Hashira.Core.DamageHandler;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class AddDamageHandlerEffect : Effect, ICoolTimeEffect
    {
        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }

        private EntityHealth _entityHealth;

        private DamageHandler _damageHandler;
        private EDamageHandlerLayer _damageHandlerLayer;

        public void Setup(float duration, DamageHandler handler, EDamageHandlerLayer layer)
        {
            Duration = duration;
            _damageHandler = handler;
            _damageHandlerLayer = layer;
        }

        public override void Enable()
        {
            base.Enable();
            _entityHealth = entity.GetEntityComponent<EntityHealth>();
            _entityHealth.AddDamageHandler(_damageHandlerLayer, _damageHandler);
        }

        public override void Disable()
        {
            base.Disable();
            _entityHealth.RemoveDamageHandler(_damageHandlerLayer, _damageHandler);
        }

        public void OnTimeOut()
        {
        }
    }
}
