using DG.Tweening;
using Hashira.Combat;
using Hashira.Entities.Components;
using Hashira.MainScreen;
using Hashira.Pathfind;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemStampPattern : GiantGolemPattern
    {
        [SerializeField] private float _handMaxPositionY = 1;
        [SerializeField] private AttackVisualizer _groundAttackVisualizer;
        [SerializeField] private ParticleSystem _groundParticle;

        public override void OnStart()
        {
            base.OnStart();
            _groundAttackVisualizer.ResetDamageCastVisualSign();
            _giantGolemEye.LookAtPlayerDirection = false;
            EntityAnimator.OnAnimationTriggeredEvent += HandleAttackEnd;
            EntityAnimator.OnAnimationTriggeredEvent += HandleScream;
        }

        private void HandleScream(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Start)
                SoundManager.Instance.PlaySFX("GiantGolemScream", Transform.position, 1);
        }

        private void HandleAttackEnd(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Trigger)
            {
                Sequence seq = DOTween.Sequence();
                seq.Append(_handL.transform.DOMoveY(_handMaxPositionY, 0.5f));
                seq.Join(_handR.transform.DOMoveY(_handMaxPositionY, 0.5f));
                seq.AppendInterval(1.25f);
                seq.Join(_groundAttackVisualizer.SetDamageCastSignValue(1, 1.25f));
                seq.Append(_handL.transform.DOMoveY(Boss.GetGroundFloorPosY() + 2, 0.1f));
                seq.Join(_handR.transform.DOMoveY(Boss.GetGroundFloorPosY() + 2, 0.1f));
                seq.AppendCallback(() =>
                {
                    SoundManager.Instance.PlaySFX("GiantGolemStamp", Transform.position, 1);
                });
                seq.Join(_groundAttackVisualizer.SetDamageCastSignValue(0, 0.1f));
                seq.AppendCallback(() =>
                {
                    _groundAttackVisualizer.DamageCaster.CastDamage(AttackInfo.defaultOneDamage, Vector2.up * 30);
                    _groundParticle.Play(true);
                });
                seq.Append(MainScreenEffect.OnLocalMoveScreenSide(DirectionType.Down));
                seq.JoinCallback(() => CameraManager.Instance.ShakeCamera(30, 20, 1.45f));
                seq.AppendInterval(1.4f);
                seq.Append(MainScreenEffect.OnLocalMoveScreenSide(DirectionType.Zero));
            }


            if (trigger == EAnimationTriggerType.End)
            {
                EndPattern();
            }
        }

        public override void OnEnd()
        {
            _giantGolemEye.LookAtPlayerDirection = false;
            EntityAnimator.OnAnimationTriggeredEvent -= HandleScream;
            EntityAnimator.OnAnimationTriggeredEvent -= HandleAttackEnd;
            base.OnEnd();
        }

        public override void OnDrawGizmos(Transform transform)
        {
            base.OnDrawGizmos(transform);
            Gizmos.DrawLine(new Vector3(-1000, _handMaxPositionY), new Vector3(1000, _handMaxPositionY));
        }
    }
}
