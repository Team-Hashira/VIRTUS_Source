using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class TacticalShootCard : CardEffect
    {
        private StatElement _damageStat;
        private float _damageIncrease;
        [SerializeField] private float _damageUpValue = 10;
        [SerializeField] private float _damageDownValue = 20;
        [SerializeField] private Vector2 _damageIncreaseClamp = new Vector2(-50, 100);

        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);

            _damageIncrease = 0;
            _damageStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            if (projectileHitEvent.hitInfo.damageable != null)
            {
                _damageIncrease += _damageUpValue;
            }
            else
            {
                _damageIncrease -= _damageDownValue;
            }
            _damageIncrease = Mathf.Clamp(_damageIncrease, -_damageIncreaseClamp.x, _damageIncreaseClamp.y);
            _damageStat.AddModify(nameof(TacticalShootCard), _damageIncrease, EModifyMode.Percent, EModifyLayer.Default, false);
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }
    }
}
