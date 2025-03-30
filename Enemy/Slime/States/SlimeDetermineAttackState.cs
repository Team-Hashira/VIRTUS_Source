using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.Slime
{
    public class SlimeDetermineAttackState : EntityState
    {
        private Slime _slime;

        private EntityRenderer _entityRenderer;
        private EnemyDetector _enemyDetector;

        private Player _target;

        public SlimeDetermineAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _slime = entity as Slime;

            _entityRenderer = entity.GetEntityComponent<EntityRenderer>();
            _enemyDetector = entity.GetEntityComponent<EnemyDetector>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            
            _entityRenderer.LookTarget(_target.transform.position);

            if (_enemyDetector.IsTargetOnAttackRange(_target.transform, _slime.DashAttackRange)) // DashAttack이 우선순위.
                _entityStateMachine.ChangeState("DashAttack");
            else
                _entityStateMachine.ChangeState("ShootBall");
        }
    }
}
