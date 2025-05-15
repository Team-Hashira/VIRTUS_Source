using Hashira.Combat;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hashira.EffectSystem.Effects
{
    public class Bleeding : Effect, ICoolTimeEffect
    {
        private int _damage;
        private float _damageDelay;
        private float _lastDamageTime;

        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }

        public string Key { get; private set; }

        public void Setup(int damage, float damageDelay, string key)
        {
            _damage = damage;
            _damageDelay = damageDelay;
            Duration = 3f;
            Key = key;
        }

        public override void Enable()
        {
            base.Enable();
            _lastDamageTime = 0;
        }

        public override void Update()
        {
            base.Update();


            if (_lastDamageTime + _damageDelay < Time.time)
            {
                _lastDamageTime = Time.time;
                bool isOverBleeding = entityEffector.GetEffectList(this).Any(bleeding => (bleeding as Bleeding).Key != Key);
                int damage = isOverBleeding ? _damage * 2 : _damage;
                AttackInfo attackInfo = new AttackInfo(damage, Vector2.zero, EAttackType.Bleeding);
                entity.GetEntityComponent<EntityHealth>().ApplyDamage(attackInfo);
            }
        }

        public void OnTimeOut()
        {

        }
    }
}
