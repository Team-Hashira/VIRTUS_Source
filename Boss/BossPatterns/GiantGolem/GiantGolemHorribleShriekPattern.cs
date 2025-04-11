using DG.Tweening;
using Hashira.Entities.Components;
using System;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemHorribleShriekPattern : GiantGolemPattern
    {
        [SerializeField] private float _playerPushStrength = 5;
        private bool IsKnockback { get; set; }
        public float knockbackTime = 0.2f;
        private float _currentknockbackTime = 0;
        private Vector2 _knockbackDirection;
        
        public override void OnStart()
        {
            base.OnStart();
            EntityAnimator.OnAnimationTriggeredEvent += OnAnimationTriggeredHandle;
        }

        private void OnAnimationTriggeredHandle(EAnimationTriggerType triggertype, int count)
        {
            switch (triggertype)
            {
                case EAnimationTriggerType.Start:
                    break;
                case EAnimationTriggerType.Trigger:
                {
                    Vector2 dir = (Player.transform.position - Transform.position).normalized;
                    Player.Mover.StopImmediately();
                    Player.Mover.isManualMove = false;
                    _knockbackDirection = dir.normalized * _playerPushStrength;
                    IsKnockback = true;
                    CameraManager.Instance.ShakeCamera(100, 100, knockbackTime + 1, Ease.OutBack);
                }
                break;
                case EAnimationTriggerType.End:
                {
                    EndPattern();
                }
                break;
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (IsKnockback)
            {
                Player.Mover.SetMovement(_knockbackDirection, true);

                _currentknockbackTime += Time.deltaTime;
                if (_currentknockbackTime > knockbackTime)
                {
                    _currentknockbackTime = 0;
                    IsKnockback = false;
                    Player.Mover.isManualMove = true;
                    Player.Mover.StopImmediately();
                }
            }
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= OnAnimationTriggeredHandle;
            base.OnEnd();
        }
    }
}
