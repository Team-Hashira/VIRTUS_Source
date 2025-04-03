using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.FSM;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Enemies.Bee.QueenBee
{
    public class QueenBeeEvasionState : EntityState
    {
        private Player _target;

        private EnemyMover _enemyMover;

        private StatElement _dashElement;

        public QueenBeeEvasionState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();

            var stat = entity.GetEntityComponent<EntityStat>();
            _dashElement = stat.StatDictionary[StatName.DashSpeed];
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log("Evasioned");
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            Vector3 dir = _target.transform.position - _entity.transform.position;
            Debug.Log(dir);
            _enemyMover.SetMovement(-dir.normalized * _dashElement.IntValue);

            _entityStateMachine.DelayedChangeState("Idle");
        }
    }
}
