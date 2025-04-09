using Hashira.Combat;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinSwingPattern : BossPattern
    {
        [SerializeField] private CircleDamageCaster2D _damageCaster;
        private float _casterOrigin;
        [SerializeField] private float _knockbackPower = 5f;
        private float _lookDirection;

        public override void OnStart()
        {
            base.OnStart();
            _lookDirection = Mathf.Sign(Player.transform.position.x - Transform.position.x);
            EntityRenderer.LookTarget(_lookDirection);

            EntityAnimator.OnAnimationTriggeredEvent += HandleSwingAttack;
            EntityAnimator.OnAnimationTriggeredEvent += HandleEndSwingAnimation;
        }

        private void HandleSwingAttack(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                CameraManager.Instance.ShakeCamera(30, 5, 0.85f);
                _damageCaster.CastDamage(AttackInfo.defaultOneDamage, Vector2.right * _lookDirection * _knockbackPower, false);
            }
        }

        private void HandleEndSwingAnimation(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.End)
                EndPattern();
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= HandleSwingAttack;
            EntityAnimator.OnAnimationTriggeredEvent -= HandleEndSwingAnimation;

            base.OnEnd();
        }
    }
}
