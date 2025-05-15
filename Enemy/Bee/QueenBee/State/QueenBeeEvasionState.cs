using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using System;
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
            _target = _entityStateMachine.GetShareVariable<Player>("Target");
            Vector3 dir = _target.transform.position - _entity.transform.position;
            _enemyMover.SetMovement(-dir.normalized * _dashElement.IntValue);
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggered;
        }

        private void HandleOnAnimationTriggered(EAnimationTriggerType triggerType, int count)
        {
            if (triggerType == EAnimationTriggerType.Start)
            {
                _enemyMover.StopImmediately(true);
            }
            if (triggerType == EAnimationTriggerType.End)
            {
                _entityStateMachine.ChangeState("Idle");
            }
        }

        public override void OnExit()
        {
            Debug.Log("님아");
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggered;
            base.OnExit();
        }
    }
}
