using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class BombProjectileCard : CardEffect
    {
        [SerializeField] private float[] _radius = { 2, 2, 4 };
        [SerializeField] private int[] _damage = { 20, 30, 40 };
        [SerializeField] private int[] _percent = { 5, 5, 10 };

        private List<Projectile> _bombProjectileLisr;

        public override void Enable()
        {
            base.Enable();
            _bombProjectileLisr = new List<Projectile>();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileAfterHitEvent);
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleProjectileShootEvent);
        }

        private void HandleProjectileShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            if (RandomUtility.RollChance(_percent[stack - 1]))
            {
                Projectile projectile = projectileShootEvent.projectile;
                _bombProjectileLisr.Add(projectile);
                TrailData trailData = new TrailData()
                {
                    startColor = Color.red,
                    endColor = Color.red
                };
                projectile.SetVisual(Color.white, trailData, scaleMultiplier: 0.6f * _radius[stack - 1]);
            }
        }

        private void HandleProjectileAfterHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;

            if (_bombProjectileLisr.Contains(projectile))
            {
                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(projectileHitEvent.hitInfo.raycastHit.point, _radius[stack - 1], Vector2.zero, 0, projectile.WhatIsTarget);
                foreach (var hit in raycastHit2Ds)
                {
                    if (hit.transform.TryGetComponent(out Entity entity) &&
                        entity.TryGetEntityComponent(out EntityHealth health))
                    {
                        AttackInfo attackInfo = new AttackInfo(_damage[stack - 1], attackType: EAttackType.Fire);
                        health.ApplyDamage(attackInfo);
                    }
                }
                CameraManager.Instance.ShakeCamera(8, 8, 0.3f);

                ParticleSystem bombEffect = PopCore.Pop(EffectPoolType.Bomb, projectileHitEvent.hitInfo.raycastHit.point, Quaternion.identity).gameObject.GetComponent<ParticleSystem>();
                var mainModule = bombEffect.main;
                mainModule.startSize = 2.5f * _radius[stack - 1];

                SoundManager.Instance.PlaySFX("Explosion", null, 1f);

                if (projectileHitEvent.projectile.TryGetCurrentLifeCountType(out EProjectileUndyingMode projectileUndyingMode) == false)
                    _bombProjectileLisr.Remove(projectile);
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileAfterHitEvent);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleProjectileShootEvent);
        }
    }
}
