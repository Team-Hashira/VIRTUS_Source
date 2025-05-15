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
        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            RaycastHit2D raycastHit = projectileHitEvent.hitInfo.raycastHit;
            Projectile originProjectile = projectileHitEvent.projectile;

            Vector3 dir = raycastHit.normal;
            float angle = 45f / stack;
            for (int i = 0; i < stack + 1; i++)
            {
                Projectiles.Player.PlayerBullet bullet = PopCore.Pop(ProjectilePoolType.Bullet, raycastHit.point, Quaternion.identity) as Projectiles.Player.PlayerBullet;
                bullet.Init(originProjectile.WhatIsTarget, Quaternion.Euler(0, 0, -22.5f + angle * i) * dir, originProjectile.Speed,
                    Mathf.CeilToInt(originProjectile.damage * 0.3f), originProjectile.Owner, false, 3);
                bullet.SetVisual(scaleMultiplier: 0.3f);
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }
    }
}
