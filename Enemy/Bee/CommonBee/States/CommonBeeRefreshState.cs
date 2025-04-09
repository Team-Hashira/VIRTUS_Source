using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using System;
using UnityEngine;

namespace Hashira.Enemies.Bee.CommonBee
{
    public class CommonBeeRefreshState : EntityState
    {
        public CommonBeeRefreshState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityAnimator.OnAnimationTriggeredEvent += HandleOnAnimationTriggerEvent;
        }

        private void HandleOnAnimationTriggerEvent(EAnimationTriggerType triggerType, int count)
        {
            if (triggerType == EAnimationTriggerType.End)
                _entityStateMachine.ChangeState("Chase");
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggerEvent;
            base.OnExit();
        }
    }
}
