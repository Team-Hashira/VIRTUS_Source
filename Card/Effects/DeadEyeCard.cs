using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class DeadEyeCard : CardEffect
    {
        [SerializeField] private float[] _distanceForDamage = { 1f, 3f, 7f };

        private StatElement _attackDamageStat;

        public override void Enable()
        {
            GameEventChannel.AddListener<ProjectileBeginHitEvent>(HandleProjectileBeginHitEvent);
            _attackDamageStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
        }

        private void HandleProjectileBeginHitEvent(ProjectileBeginHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;

            projectile.damage = Mathf.CeilToInt(_attackDamageStat.Value * (projectile.MoveDistance * _distanceForDamage[stack - 1] / 100 + 1));
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileBeginHitEvent>(HandleProjectileBeginHitEvent);
        }

        public override void Update()
        {

        }
    }
}
