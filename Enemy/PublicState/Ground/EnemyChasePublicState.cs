using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.PublicStates
{
    public class EnemyChasePublicState : EntityState
    {
        private EnemyMover _enemyMover;
        private EntityRenderer _entityRenderer;
        private EnemyDetector _enemyDetector;

        private StatElement _speedElement;

        private Player _target;

        public string TargetState { get; set; } = "Attack";

        public EnemyChasePublicState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();

            var stat = entity.GetEntityComponent<EntityStat>();
            _speedElement = stat.StatDictionary[StatName.Speed];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target ??= _entityStateMachine.GetShareVariable<Player>("Target");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _entityRenderer.LookTarget(_target.transform.position);
            Vector3 moveDir = new Vector2(_entityRenderer.FacingDirection * _speedElement.Value, 0);
            _enemyMover.SetMovement(_entity.transform.rotation * moveDir);
            if (_enemyDetector.IsTargetOnAttackRange(_target.transform))
            {
                _entityStateMachine.ChangeState(TargetState);
            }
        }
    }
}
