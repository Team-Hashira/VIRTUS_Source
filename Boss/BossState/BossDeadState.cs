using Hashira.Entities;
using Hashira.Entities.Components;
using Hashira.FSM;
using UnityEngine;

namespace Hashira.Bosses.States
{
    public class BossDeadState : EntityState
    {
        public BossDeadState(Entity entity, StateSO stateSO) : base(entity, stateSO)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            (_entity as Boss)?.CurrentBossPattern?.EndPattern();
            _entityAnimator.OnAnimationTriggeredEvent += HandleAnimationTriggered;
        }

        private void HandleAnimationTriggered(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.End)
            {
                _entityAnimator.OnAnimationTriggeredEvent -= HandleAnimationTriggered;
            }
        }
    }
}
