using Hashira.Core.MoveSystem;
using Hashira.Core.StatSystem;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies.Bee
{
    public class BeeAttackState : EntityState
    {
        private EnemyMover _enemyMover;

        private StatElement _dashSpeedElement;

        private Player _target;

        private Vector2 _direction;

        public BeeAttackState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _enemyMover = entity.GetEntityComponent<EnemyMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _enemyMover.GetMoveProcessor<XSmoothProcessor>().Speed = 5f;
            _enemyMover.GetMoveProcessor<YSmoothProcessor>().Speed = 5f;

            var stat = _entity.GetEntityComponent<EntityStat>();
            _dashSpeedElement = stat.StatDictionary[StatName.DashSpeed];

            _target = _entityStateMachine.GetShareVariable<Player>("Target");

            _direction = _target.transform.position - _entity.transform.position;
            _direction.Normalize();

            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType triggerType, int count)
        {
            if(triggerType == EAnimationTriggerType.Trigger)
            {
                _enemyMover.SetMovement(_direction * _dashSpeedElement.Value);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            _enemyMover.GetMoveProcessor<XSmoothProcessor>().Speed = 2f;
            _enemyMover.GetMoveProcessor<YSmoothProcessor>().Speed = 2f;
        }
    }
}
