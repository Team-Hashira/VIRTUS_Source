using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class TaserGunCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 2 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private int _currentShootCount = 0;
        private int _needShootCount = 15;
        private int _damage = 50;

        private List<Projectile> _projectileList;

        public override void Enable()
        {
            _currentShootCount = 0;
            _projectileList = new List<Projectile>();
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleShootEvent);
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleHitEvent);
        }

        private void TaserDamage(HitInfo hitInfo)
        {
            PopCore.Pop(EffectPoolType.TaserEffect, hitInfo.raycastHit.point, Quaternion.identity);
            if (hitInfo.entity != null)
            {
                Stun stun = new Stun();
                stun.Setup(2f);
                hitInfo.entity.GetEntityComponent<EntityEffector>().AddEffect(stun);
            }

            RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(hitInfo.raycastHit.point, 3f, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {
                    if (raycastHit.transform.TryGetComponent(out Entity entity))
                    {
                        Slowdown slowdown = new Slowdown();
                        StatModifier statModifier = new StatModifier(-30, EModifyMode.Percent, false);
                        slowdown.Setup(statModifier, 2f);
                        entity.GetEntityComponent<EntityEffector>().AddEffect(slowdown);
                    }
                    damageable.ApplyDamage(_damage, raycastHit, player.transform, attackType: EAttackType.Electricity);
                }
            }
        }

        private void HandleHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;
            if (_projectileList.Contains(projectile) == false) return;

            if (projectile.TryGetCurrentLifeCountType(out EProjectileUndyingMode projectileUndyingMode) == false)
                _projectileList.Remove(projectile);

            HitInfo hitInfo = projectileHitEvent.hitInfo;

            TaserDamage(hitInfo);

            if (stack < 2) return;

            RaycastHit2D[] raycastHits = Physics2D.CircleCastAll(player.transform.position, 3f, Vector2.zero, 0, 1 << LayerMask.NameToLayer("Enemy"));
            foreach (RaycastHit2D raycastHit in raycastHits)
            {
                if (raycastHit.transform.TryGetComponent(out IDamageable damageable))
                {
                    Entity entity = raycastHit.transform.GetComponent<Entity>();
                    HitInfo newHitInfo = new HitInfo()
                    {
                        raycastHit = raycastHit,
                        damageable = damageable,
                        entity = entity,
                    };
                    TaserDamage(newHitInfo);
                    break;
                }
            }
        }

        private void HandleShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            _currentShootCount++;
            if (_currentShootCount >= _needShootCount)
            {
                Projectile projectile = projectileShootEvent.projectile;
                _currentShootCount = 0;

                Color color = EnumUtility.AttackTypeColorDict[EAttackType.Electricity];
                TrailData trailData = new TrailData();
                trailData.startColor = color;
                trailData.endColor = color;
                projectile.SetVisual(color, trailData);

                _projectileList.Add(projectile);
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleHitEvent);
        }

        public override void Update()
        {

        }
    }
}
