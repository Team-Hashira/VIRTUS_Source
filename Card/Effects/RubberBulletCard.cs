using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class RubberBulletCard : CardEffect
    {
        [SerializeField] private int[] _redirectionCountByStack = { 1, 1, 2 };
        [SerializeField] private float[] _attackDamageDownByStack = { 20f, 10f, 10f };

        private StatElement _attackPower;

        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleProjectileShootEvent);
            _attackPower = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
            _attackPower.AddModify("RubberBulletCard", -_attackDamageDownByStack[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        private void HandleProjectileShootEvent(ProjectileShootEvent projectileShootEvent)
        {
            projectileShootEvent.projectile.SetLifeCount(EProjectileUndyingMode.Reflection, _redirectionCountByStack[stack - 1]);
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            Projectile projectile = projectileHitEvent.projectile;

            if (projectile.TryGetCurrentLifeCountType(out EProjectileUndyingMode eProjectileUndyingMode) &&
                eProjectileUndyingMode == EProjectileUndyingMode.Reflection)
            {
                if (projectile.GetLifeCount(EProjectileUndyingMode.Reflection) > 0)
                {
                    Vector2 reflect = Vector2.Reflect(projectile.movement, projectileHitEvent.hitInfo.raycastHit.normal);
                    reflect.Normalize();
                    projectile.Redirection(reflect);
                    projectile.damage = Mathf.CeilToInt(projectile.damage * 0.8f);
                    projectile.transform.position += (Vector3)reflect * projectile.Speed * Time.deltaTime;
                }
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleProjectileShootEvent);
            _attackPower.RemoveModifyOverlap("RubberBulletCard", EModifyLayer.Default);
        }
    }
}
