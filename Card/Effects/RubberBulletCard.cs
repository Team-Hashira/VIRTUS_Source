using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class RubberBulletCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        public float Duration { get; set; } = 5;
        public float Amount { get; set; }

        private int[] _redirectionCountByStack = { 1, 1, 2 };
        private float[] _attackDamageDownByStack = { 20f, 10f, 10f };

        private StatElement _attackPower;

        public override void Enable()
        {
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
                    projectile.transform.position += (Vector3)reflect * projectile.Speed * Time.deltaTime;
                }
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleProjectileShootEvent);
            _attackPower.RemoveModify("RubberBulletCard", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
