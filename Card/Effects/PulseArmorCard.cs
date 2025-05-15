using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.EventSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class PulseArmorCard : CardEffect
    {
        [SerializeField] private int[] _damageByStack = { 50, 70, 70 };
        [SerializeField] private float[] _radiusByStack = { 3, 3, 6 };

        private AttackInfo _attackInfo;

        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<PlayerHitEvent>(HandlePlayerHitEvent);
            if (false == IsMaxStack)
                _attackInfo = new AttackInfo(_damageByStack[stack - 1], attackType: Entities.EAttackType.Fixed, attacker: player);
        }

        private void HandlePlayerHitEvent(PlayerHitEvent playerHitEvent)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(player.transform.position, _radiusByStack[stack - 1], Vector3.zero, 0, player.Attacker.WhatIsTarget);

            Stun stun = new Stun();
            stun.Setup(0.1f);
            ParticleSystem effect = PopCore.Pop(CardSubPoolType.PulseAttack, player.transform).gameObject.GetComponent<ParticleSystem>();
            var mainModule = effect.main;
            mainModule.startSize = _radiusByStack[stack - 1];

            foreach (var hit in hits)
            {
                if (hit.transform.TryGetComponent(out IDamageable damageable))
                {
                    if (IsMaxStack)
                    {
                        if (damageable is EntityHealth health)
                            health.Owner.GetEntityComponent<EntityEffector>().AddEffect(stun);
                        Vector2 knockback = (hit.transform.position - player.transform.position).normalized * 10f;
                        AttackInfo attackInfo = new AttackInfo(_damageByStack[stack - 1], knockback, Entities.EAttackType.Fixed, player);
                        damageable.ApplyDamage(attackInfo, hit);
                    }
                    else
                        damageable.ApplyDamage(_attackInfo, hit);
                }
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<PlayerHitEvent>(HandlePlayerHitEvent);
        }
    }
}
