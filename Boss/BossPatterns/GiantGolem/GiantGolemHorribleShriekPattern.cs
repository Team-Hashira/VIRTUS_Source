using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Entities.Components;
using Hashira.LightingControl;
using Unity.Mathematics;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHorribleShriekPattern : GiantGolemPattern
    {
        [SerializeField] private EffectPoolType _waveEffectPoolType;
        [SerializeField] private float _waveEffectDelay = 0.1f;
        private float _waveEffectTimer = 0;
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
                    SoundManager.Instance.PlaySFX("GiantGolemStamp", Transform.position, 1);
                    SoundManager.Instance.PlaySFX("GiantGolemScream", Transform.position, 1);
                    Vector2 dir = (Player.transform.position - Transform.position).normalized;
                    Player.Mover.StopImmediately();
                    Player.Mover.isManualMove = false;
                    _knockbackDirection = dir.normalized * _playerPushStrength;
                    IsKnockback = true;
                    _waveEffectTimer = Time.time;
                    CameraManager.Instance.ShakeCamera(100, 100, knockbackTime + 1, Ease.OutBack);
                    LightingController.Aberration(1f, 0.3f);
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
                if (_waveEffectTimer + _waveEffectDelay < Time.time)
                {
                    GameObject.Pop(_waveEffectPoolType, Transform.position, quaternion.identity);
                    _waveEffectTimer = Time.time;
                }
                
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

        public override void OnDie()
        {
            _currentknockbackTime = 0;
            IsKnockback = false;
            Player.Mover.isManualMove = true;
            Player.Mover.StopImmediately();
            base.OnDie();
        }
    }
}
