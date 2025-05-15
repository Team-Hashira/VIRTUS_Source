using Hashira.Combat;
using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.Core.StatSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Enemies;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Hashira.Cards.Effects
{
    public class StormsurgeCard : CardEffect
    {
        private Dictionary<Entity, Pair<float, int>> _enemyHitTime;
        private float _enableTime = 2.5f;

        private AttackInfo _attackInfo;
        private StatElement _attackSpeedStat;

        private int[] _damageByStack = { 40, 50, 70 };

        private float _lastAttackSpeedIncreasesTime;
        private float _attackSpeedIncreasesDuration = 2;
        private bool _isIncreasesedAttackSpeed;

        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
            _enemyHitTime = new Dictionary<Entity, Pair<float, int>>();

            _attackInfo = new AttackInfo(_damageByStack[stack - 1], attackType: EAttackType.Fixed);
            _attackSpeedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackSpeed];
            _lastAttackSpeedIncreasesTime = 0;
            _isIncreasesedAttackSpeed = false;
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            if (projectileHitEvent.hitInfo.entity != null &&
                _enemyHitTime.ContainsKey(projectileHitEvent.hitInfo.entity) == false)
            {
                EntityHealth entityHealth = projectileHitEvent.hitInfo.entity.GetEntityComponent<EntityHealth>();
                _enemyHitTime[projectileHitEvent.hitInfo.entity] = new Pair<float, int>(Time.time, entityHealth.Health);
                entityHealth.OnHealthChangedEvent += HandleHealthChangedEvent;
                entityHealth.OnDieEvent += HandleDieEvent;
            }
        }

        private void HandleDieEvent(Entity entity)
        {
            _enemyHitTime.Remove(entity);
            EntityHealth entityHealth = entity.GetEntityComponent<EntityHealth>();
            entityHealth.OnHealthChangedEvent -= HandleHealthChangedEvent;
            entityHealth.OnDieEvent -= HandleDieEvent;
        }

        private void HandleHealthChangedEvent(int previous, int current)
        {
            List<Entity> entityList = _enemyHitTime.Keys.ToList();
            foreach (var enemy in entityList)
            {
                EntityHealth entityHealth = enemy.GetEntityComponent<EntityHealth>();

                // 제한시간 초과
                if (_enemyHitTime[enemy].first + _enableTime < Time.time)
                {
                    entityHealth.OnHealthChangedEvent -= HandleHealthChangedEvent;
                    entityHealth.OnDieEvent -= HandleDieEvent;
                    continue;
                }
                // 20% 타격
                if ((float)(_enemyHitTime[enemy].second - entityHealth.Health) / entityHealth.MaxHealth > 0.2f)
                {
                    entityHealth.OnHealthChangedEvent -= HandleHealthChangedEvent;
                    entityHealth.OnDieEvent -= HandleDieEvent;

                    entityHealth.ApplyDamage(_attackInfo);
                    Stun stun = new Stun();
                    stun.Setup(0.5f);
                    enemy.GetEntityComponent<EntityEffector>().AddEffect(stun);

                    // 공속증가 중복 안되도록 적용
                    if (false == _isIncreasesedAttackSpeed)
                    {
                        _isIncreasesedAttackSpeed = true;
                        _attackSpeedStat.AddModify(nameof(StormsurgeCard), 10f, EModifyMode.Percent, EModifyLayer.Default, false);
                    }
                    _lastAttackSpeedIncreasesTime = Time.time;
                }
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        public override void Update()
        {
            // 공속증가 쿨 끝났으면 제거
            if (_isIncreasesedAttackSpeed && _lastAttackSpeedIncreasesTime + _attackSpeedIncreasesDuration < Time.time)
            {
                _isIncreasesedAttackSpeed = false;
                _attackSpeedStat.RemoveModify(nameof(StormsurgeCard), EModifyLayer.Default);
            }
        }
    }
}
