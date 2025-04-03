using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    public class AirEnemyChaseState : EntityState
    {
        private Player _target;

        private EnemyMover _enemyMover;
        private EnemyDetector _enemyDetector;
        private EntityRenderer _entityRenderer;

        private StatElement _speedElement;

        public string TargetState { get; set; } = "Attack";

        public AirEnemyChaseState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();

            var stat = entity.GetEntityComponent<EntityStat>();
            _speedElement = stat.StatDictionary[StatName.Speed];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            Vector2 dir = _target.transform.position - _entity.transform.position;
            dir.Normalize();
            _enemyMover.SetMovement(dir * _speedElement.Value);
            _entityRenderer.LookTarget(_target.transform.position);
            if (_enemyDetector.IsTargetOnAttackRange(_target.transform))
            {
                _entityStateMachine.ChangeState(TargetState);
            }
        }
    }
}
