using Hashira.Core.EventSystem;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class PenetratingProjectileCard : CardEffect
    {
        [SerializeField] private float[] _penetratingPercentByStack = { 30f, 40f, 50f };
        [SerializeField] private float[] _damageDownByStack = { 30f, 15f, 0f };

        public override void Enable()
        {
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleShootEvent);
        }

        private void HandleShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            float random = Random.Range(0f, 100f);
            if (random < _penetratingPercentByStack[stack - 1])
            {
                Projectile projectile = projectileShootEvent.projectile;
                projectile.SetLifeCount(EProjectileUndyingMode.penetration, 1);
            }
        }

        private void HandleHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;
            if (projectile.TryGetCurrentLifeCountType(out EProjectileUndyingMode projectileUndyingMode) &&
                projectileUndyingMode == EProjectileUndyingMode.penetration)
            {
                projectile.damage = Mathf.CeilToInt(projectile.damage * (100 - _damageDownByStack[stack - 1]) / 100);
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
        }

        public override void Update()
        {

        }
    }
}
