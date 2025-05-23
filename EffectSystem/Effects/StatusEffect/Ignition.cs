using Hashira.Combat;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Ignition : Effect, ICoolTimeEffect
    {
        private int _damage;
        private float _damageDelay = 0.5f;
        private float _lastDamageTime;

        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }

        public void Setup(int damage, float damageDelay, float duration)
        {
            _damage = damage;
            _damageDelay = damageDelay;
            Duration = duration;
        }

        public override void Enable()
        {
            base.Enable();
            _lastDamageTime = Time.time;
        }

        public override void Update()
        {
            base.Update();
            if (_lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
                AttackInfo attackInfo = new AttackInfo(_damage, Vector2.zero, EAttackType.Fire);
               entity.GetEntityComponent<EntityHealth>().ApplyDamage(attackInfo);
            }
        }

        public override void Disable()
        {
            base.Disable();
        }

        public void OnTimeOut()
        {

        }
    }
}
