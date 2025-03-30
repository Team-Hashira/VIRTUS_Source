using Crogen.CrogenPooling;
using Hashira.Enemies.Components;
using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using Hashira.Players;
using System;
using UnityEngine;

namespace Hashira.Enemies.Slime
{
    public class SlimeShootBallState : EntityState
    {
        private Slime _slime;

        private EnemyMover _enemyMover;

        private Player _target;

        public SlimeShootBallState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
            _slime = entity as Slime;

            _enemyMover = entity.GetEntityComponent<EnemyMover>();
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _target = _entityStateMachine.GetShareVariable<Player>("Target");

            _enemyMover.StopImmediately();
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggeredEvent;
        }

        private void HandleOnAnimationTriggeredEvent(EAnimationTriggerType triggerType, int count)
        {
            if (triggerType == EAnimationTriggerType.Trigger)
            {
                SlimeBall slimeBall = PopCore.Pop(_slime.SlimeBall, _slime.transform.position, Quaternion.identity) as SlimeBall;
                slimeBall.Initialize(_slime, _target.transform.position);
            }
            if(triggerType == EAnimationTriggerType.End)
            {
                _entityStateMachine.DelayedChangeState("Chase");
            }
        }
    }
}
