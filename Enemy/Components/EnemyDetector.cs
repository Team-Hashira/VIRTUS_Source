using Hashira.Core.StatSystem;
using Hashira.Entities;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.Components
{
    public class EnemyDetector : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent
    {
        private Enemy _enemy;

        private EntityStat _entityStat;

        [Header("Detect Setting")]
        [SerializeField]
        private LayerMask _whatIsGround;

        private StatElement _sightElement;
        private StatElement _attackRangeElement;

        public void Initialize(Entity entity)
        {
            _enemy = entity as Enemy;

            _entityStat = entity.GetEntityComponent<EntityStat>();
        }

        public void AfterInit()
        {
            _sightElement = _entityStat.StatDictionary[StatName.Sight];
            _attackRangeElement = _entityStat.StatDictionary[StatName.AttackRange];
        }

        public virtual Player DetectPlayer()
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, _sightElement.Value, _enemy.WhatIsPlayer);
            if (coll == null)
                return null;
            Vector3 direction = coll.transform.position - transform.position;
            float distance = direction.magnitude;
            direction.Normalize();
            //if (!Physics2D.Raycast(transform.position, direction, distance, _whatIsGround))
            {
                return coll.GetComponent<Player>();
            }
        }

        public virtual bool IsWallBetweenTarget(Transform target)
        {
            Vector3 direction = target.position - transform.position;
            return Physics2D.Raycast(transform.position, direction.normalized, direction.magnitude, _whatIsGround);
        }

        public virtual bool IsTargetOnAttackRange(Transform target)
        {
            float distanceSqr = (transform.position - target.position).sqrMagnitude;
            float attackRangeSqr = _attackRangeElement.Value * _attackRangeElement.Value;

            return distanceSqr < attackRangeSqr;
        }

        public virtual bool IsTargetOnAttackRange(Transform target, float attackRange)
        {
            float distanceSqr = (transform.position - target.position).sqrMagnitude;
            float attackRangeSqr = attackRange * attackRange;

            return distanceSqr < attackRangeSqr;
        }

    }
}
