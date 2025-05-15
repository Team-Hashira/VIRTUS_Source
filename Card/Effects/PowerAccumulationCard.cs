using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class PowerAccumulationCard : CardEffect
    {
        [SerializeField] private float[] _accumulationDelay = { 10, 10, 8 };
        [SerializeField] private int[] _bombDamage = { 60, 70, 70 };
        [SerializeField] private int _radius = 2;

        private float _lastAccumulationShootTime;
        private bool _isAccumulated;

        private List<Projectile> _accumulatedProjectileList;

        public override void Enable()
        {
            base.Enable();
            _accumulatedProjectileList = new List<Projectile>();
            _lastAccumulationShootTime = Time.time;
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        public override void Update()
        {
            if (_isAccumulated == false && _lastAccumulationShootTime + _accumulationDelay[stack - 1] < Time.time)
            {
                _isAccumulated = true;
                GameEventChannel.AddListener<ProjectileShootEvent>(HandleProjectileShootEvent);
            }
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;
            if (_accumulatedProjectileList.Contains(projectile))
            {
                _accumulatedProjectileList.Remove(projectile);

                RaycastHit2D[] raycastHit2Ds = Physics2D.CircleCastAll(projectileHitEvent.hitInfo.raycastHit.point, _radius, Vector2.zero, 0, projectile.WhatIsTarget);
                foreach (var hit in raycastHit2Ds)
                {
                    if (hit.transform.TryGetComponent(out Entity entity) &&
                        entity.TryGetEntityComponent(out EntityHealth health))
                    {
                        AttackInfo attackInfo = new AttackInfo(_bombDamage[stack - 1], attackType: EAttackType.Fire);
                        health.ApplyDamage(attackInfo);
                    }
                }
                CameraManager.Instance.ShakeCamera(8, 8, 0.3f);

                ParticleSystem bombEffect = PopCore.Pop(EffectPoolType.Bomb, projectileHitEvent.hitInfo.raycastHit.point, Quaternion.identity).gameObject.GetComponent<ParticleSystem>();
                var mainModule = bombEffect.main;
                mainModule.startSize = 2.5f * _radius;
            }
        }

        private void HandleProjectileShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            _accumulatedProjectileList.Add(projectileShootEvent.projectile);

            Color color = new Color(0.4f, 1, 0.4f, 1);
            TrailData trailData = new TrailData()
            {
                startColor = color,
                endColor = color
            };
            projectileShootEvent.projectile.SetVisual(color, trailData);
            _lastAccumulationShootTime = Time.time;
            _isAccumulated = false;
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleProjectileShootEvent);
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }
    }
}
