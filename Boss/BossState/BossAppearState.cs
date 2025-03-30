using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossAppearState : EntityState
    {
        public BossAppearState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _entityAnimator.OnAnimationTriggeredEvent += HandleAppearStateEnd;
        }

        private void HandleAppearStateEnd(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.End)
                _entityStateMachine.ChangeState("Idle");
        }

        public override void OnExit()
        {
            _entityAnimator.OnAnimationTriggeredEvent -= HandleAppearStateEnd;
            base.OnExit();
        }
    }
}
