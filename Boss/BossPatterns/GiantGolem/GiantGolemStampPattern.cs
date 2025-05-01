using DG.Tweening;
using Hashira.Bosses.BillboardClasses;
using Hashira.Combat;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemStampPattern : GiantGolemPattern
    {
        [SerializeField] private float _handMaxPositionY = 1;
        [SerializeField] private AttackVisualizer _groundAttackVisualizer;
        [SerializeField] private ParticleSystem _groundParticle;

        private Sequence _attackSequence;
        
        public override bool CanStart()
        {
            return !Boss.GetBossPattern<GiantGolemSmokeUpPattern>().IsMoved;
        }

        public override void OnStart()
        {
            base.OnStart();
            _groundAttackVisualizer.InitDamageCastVisualSign();
            _giantGolemEye.LookAtPlayerDirection = false;
            EntityAnimator.OnAnimationTriggeredEvent += HandleAttack;
        }

        private void HandleAttack(EAnimationTriggerType trigger, int count)
        {
            if (trigger == EAnimationTriggerType.Start)
                SoundManager.Instance.PlaySFX("GiantGolemScream", Transform.position, 1);
            else if (trigger == EAnimationTriggerType.Trigger)
            {
                // 손들기
                _attackSequence = DOTween.Sequence();
                if (_handL != null) _attackSequence.Append(_handL.transform.DOMoveY(_handMaxPositionY, 0.5f));
                if (_handR != null)
                {
                    if (_handL != null)
                        _attackSequence.Join(_handR.transform.DOMoveY(_handMaxPositionY, 0.5f));
                    else
                        _attackSequence.Append(_handR.transform.DOMoveY(_handMaxPositionY, 0.5f));
                }

                // 기다렸다가
                _attackSequence.AppendInterval(1.25f).Join(_groundAttackVisualizer.SetDamageCastValue(1, 1.25f));

                // 내려치기
                if (_handL != null) _attackSequence.Append(_handL.transform.DOMoveY(_groundAttackVisualizer.transform.position.y + 0.25f, 0.1f));
                if (_handR != null)
                {
                    if (_handL != null)
                        _attackSequence.Join(_handR.transform.DOMoveY(_groundAttackVisualizer.transform.position.y + 0.25f, 0.1f));
                    else
                        _attackSequence.Append(_handR.transform.DOMoveY(_groundAttackVisualizer.transform.position.y + 0.25f, 0.1f));
                }
                
                // SFX, 데미지 입히기, VFX 등 출력
                _attackSequence.AppendCallback(() =>
                    {
                        SoundManager.Instance.PlaySFX("GiantGolemStamp", Transform.position, 1);
                    })
                    .Join(_groundAttackVisualizer.SetDamageCastValue(0, 0.1f))
                    .AppendCallback(() =>
                    {
                        _groundAttackVisualizer.DamageCaster.CastDamage(AttackInfo.defaultOneDamage, popupText:false);
                        _giantGolemPlatformList.AllShake(1f, 19f, 30).SetEase(Ease.OutCubic);
                        _groundParticle.Play(true);
                    })
                    .JoinCallback(() => CameraManager.Instance.ShakeCamera(30, 20, 1.45f))
                    .AppendInterval(1.4f);
            }
            else if (trigger == EAnimationTriggerType.End)
                EndPattern();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_handR == null && _handL == null)
            {
                EndPattern();
            }
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= HandleAttack;
            _groundAttackVisualizer.SetDamageCastValue(0);
            _attackSequence?.Kill();
            if (_handL != null) _handL.transform.DOMove(_handL.BillboardValue<TransformValue>("OriginPoint").Value.position, 1f);
            if (_handR != null) _handR.transform.DOMove(_handR.BillboardValue<TransformValue>("OriginPoint").Value.position, 1f);
            _giantGolemEye.LookAtPlayerDirection = false;
            
            base.OnEnd();
        }
    }
}
