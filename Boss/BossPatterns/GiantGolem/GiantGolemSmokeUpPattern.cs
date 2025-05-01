using DG.Tweening;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemSmokeUpPattern : GiantGolemPattern
    {
        [SerializeField] private float _maxSmokePositionY;
        [SerializeField] private float _maxPlatformPositionY;
        [SerializeField] private float _maxSelfPositionY;
        private float _originSmokePositionY;
        private float _originSelfPositionY;
        [SerializeField] private float _duration = 5;
        public bool IsMoved { get; private set; } = false;

        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _originSmokePositionY =_smokeTransform.position.y;
            _originSelfPositionY = Transform.position.y;
        }

        public override void OnStart()
        {
            base.OnStart();
            EntityAnimator.OnAnimationTriggeredEvent += HandleAnimationTriggered;
        }
        
        private void HandleAnimationTriggered(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.Start)
            {
                SoundManager.Instance.PlaySFX("GiantGolemScream", Transform.position, 1);
                if (IsMoved) // 이미 움직인 상태
                {
                    Sequence seq = DOTween.Sequence();
                    seq.Append(_smokeTransform.DOMoveY(_originSmokePositionY, _duration/2).SetEase(Ease.Linear));
                    seq.Join(Transform.DOMoveY(_originSelfPositionY, _duration/2).SetEase(Ease.InOutBack));
                    seq.Join(_giantGolemPlatformList.AllMoveToOrigin(_duration/2));
                }
                else // 움직이지 않은 상태
                {
                    GiantGolemPlatform[] platforms = _giantGolemPlatformList.GetAllPlatforms();
            
                    Sequence seq = DOTween.Sequence();
                    seq.Append(_smokeTransform.DOMoveY(_maxSmokePositionY, _duration/2).SetEase(Ease.Linear));
                    seq.Join(Transform.DOMoveY(_maxSelfPositionY, _duration).SetEase(Ease.InOutBack));
                    seq.Join(PlatformUp());
                }

                IsMoved = !IsMoved;
            }
            else if (triggertype == EAnimationTriggerType.End)
            {
                 EndPattern();
            }
        }

        private Tween PlatformUp()
        {
            GiantGolemPlatform[] platforms = _giantGolemPlatformList.GetAllPlatforms();

            Sequence seq = DOTween.Sequence();
            
            for (int i = 0; i < platforms.Length; i++)
            {
                seq.Join(platforms[i].Shake(0.15f, 20f, 80));
                seq.Join(platforms[i].MoveYRandom(duration:_duration, min:_originSmokePositionY, max:_maxPlatformPositionY));
            }
            
            return seq;
        }
        
        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= HandleAnimationTriggered;
            base.OnEnd();
        }

        public override void OnDrawGizmos(Transform transform)
        {
            base.OnDrawGizmos(transform);
            Gizmos.DrawWireSphere(new Vector3(0, _maxSmokePositionY, 0), 1);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(0, _maxPlatformPositionY, 0), 1);
            Gizmos.color = Color.white;
        }
    }
}