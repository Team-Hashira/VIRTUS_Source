using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class PrecisionAimCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private StatElement _attackPowerStat;

        private float[] _attackPowerUpByStack = { 20, 40, 60, 60 };
        private float[] _criticalDamageUpByStack = { 2f, 2f, 2.5f, 2.5f };
        private float[] _damageUpPercentByStack = { 3f, 3f, 3f, 10f };

        public override void Enable()
        {
            GameEventChannel.AddListener<ProjectileShootEvent>(HandleShootEvent);
            _attackPowerStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
            _attackPowerStat.AddModify("PrecisionAimEffect", _attackPowerUpByStack[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        private void HandleShootEvent(ProjectileShootEvent projectileHitEvent)
        {
            float random = Random.Range(0f, 100f);
            if (random < _damageUpPercentByStack[stack - 1])
            {
                Projectile projectile = projectileHitEvent.projectile;
                projectile.damage = Mathf.CeilToInt(projectile.damage * _criticalDamageUpByStack[stack - 1]);
            }
        }

        public override void Disable()
        {
            _attackPowerStat.RemoveModify("PrecisionAimEffect", EModifyLayer.Default);
            GameEventChannel.RemoveListener<ProjectileShootEvent>(HandleShootEvent);
        }

        public override void Update()
        {

        }
    }
}
