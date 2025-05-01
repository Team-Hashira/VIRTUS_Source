using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Core.EventSystem;
using Hashira.Entities;
using Hashira.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class AbsenceCard : CardEffect
    {
        private Dictionary<Entity, int> _attackCountDictionary;

        [SerializeField] private float[] _damageMultiplier = { 0.5f, 0.7f, 1f };
        [SerializeField] private float[] _damageComdition = { 60f, 60f, 40f };

        public override void Enable()
        {
            _attackCountDictionary = new();
            GameEventChannel.AddListener<ProjectileBeginHitEvent>(HandleBeginHitEvent);
        }

        private void HandleBeginHitEvent(ProjectileBeginHitEvent projectileHitEvent)
        {
            if (projectileHitEvent.hitInfo.raycastHit.transform.TryGetComponent(out Entity entity) &&
                entity.TryGetEntityComponent(out EntityHealth health))
            {
                if ((float)health.Health / health.MaxHealth >= _damageComdition[stack - 1] / 100)
                {
                    if (_attackCountDictionary.ContainsKey(entity))
                        _attackCountDictionary[entity]++;
                    else
                        _attackCountDictionary[entity] = 1;

                    //if (_attackCountDictionary[entity] <= 3)
                    player.StartCoroutine(DelayAttackCoroutine(0.1f, entity.transform, health, projectileHitEvent));
                }
            }
        }

        private IEnumerator DelayAttackCoroutine(float delay, Transform target, EntityHealth health, ProjectileBeginHitEvent projectileHit)
        {
            HitInfo hitInfo = projectileHit.hitInfo;
            Projectile projectile = projectileHit.projectile;
            yield return new WaitForSeconds(delay);
            if (target != null)
            {
                Vector3 hitPos = hitInfo.raycastHit.collider.transform.position;
                RaycastHit2D raycastHit = new RaycastHit2D()
                {
                    point = hitPos
                };
                PopCore.Pop(EffectPoolType.AbsenceSlice, raycastHit.point, Quaternion.identity);
                int damage = Mathf.CeilToInt(Mathf.Log(health.Health / projectile.damage + 1, 10) * projectile.damage * _damageMultiplier[stack - 1]);
                AttackInfo attackInfo = new AttackInfo(damage);
                health.ApplyDamage(attackInfo, raycastHit);

                // 내(데미지)가 약하고 적(체력)이 강하면 추가딜 업
            }
        }

        public override void Disable()
        {
            GameEventChannel.RemoveListener<ProjectileBeginHitEvent>(HandleBeginHitEvent);
        }

        public override void Update()
        {

        }
    }
}
