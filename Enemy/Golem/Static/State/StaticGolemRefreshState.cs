using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Enemies.Golem.StaticGolem
{
    public class StaticGolemRefreshState : EntityState
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public StaticGolemRefreshState(Entity entity, StateSO stateSO) : base(entity, stateSO)
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
                _entityStateMachine.ChangeState("Attack");
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleOnAnimationTriggerEvent;
            base.OnExit();
        }
    }
}
