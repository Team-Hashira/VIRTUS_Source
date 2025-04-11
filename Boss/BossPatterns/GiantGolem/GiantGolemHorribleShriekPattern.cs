using Hashira.Entities.Components;
using Hashira.Players;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemHorribleShriekPattern : GiantGolemPattern
    {
        [SerializeField] private float _playerPushStrength = 5;
        
        public override void OnStart()
        {
            base.OnStart();
            EntityAnimator.OnAnimationTriggeredEvent += OnAnimationTriggeredHandle;
        }

        private void OnAnimationTriggeredHandle(EAnimationTriggerType triggertype, int count)
        {

            if (triggertype == EAnimationTriggerType.End)
            {
                EndPattern();
            }
            if (triggertype == EAnimationTriggerType.Trigger)
            {
                Vector2 dir = -(Transform.position - Player.transform.position).normalized;
                Player.Mover.Rigidbody2D.AddForce(dir * _playerPushStrength);
            }
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= OnAnimationTriggeredHandle;
            base.OnEnd();
        }
    }
}
