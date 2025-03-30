using Hashira.Core.EventSystem;
using UnityEngine;
using Crogen.CrogenPooling;
using Hashira.Projectiles;
using Hashira.Projectiles.Player;
using System.Collections.Generic;
using System;

namespace Hashira.Cards.Effects
{
    public class BlastBulletCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private int _splitCount;

        public override void Enable()
        {
            _splitCount = stack + 1;
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            RaycastHit2D raycastHit = projectileHitEvent.hitInfo.raycastHit;
            Projectile originProjectile = projectileHitEvent.projectile;

            Vector3 dir = raycastHit.normal;
            float angle = 45f / (_splitCount - 1);
            for (int i = 0; i < _splitCount; i++)
            {
                PlayerBullet bullet = PopCore.Pop(ProjectilePoolType.Bullet, raycastHit.point, Quaternion.identity) as PlayerBullet;
                bullet.Init(originProjectile.WhatIsTarget, Quaternion.Euler(0, 0, -22.5f + angle * i) * dir, originProjectile.Speed,
                    Mathf.CeilToInt(originProjectile.damage * 0.3f), originProjectile.Owner, false, 3);
                bullet.SetVisual(scaleMultiplier: 0.3f);
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        public override void Update()
        {
        }
    }
}
