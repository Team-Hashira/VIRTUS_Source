using Crogen.CrogenPooling;
using Hashira.Bosses.Patterns.GiantGolem;
using Hashira.Entities.Components;
using System.Collections;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHomingProjectilePattern : GiantGolemPattern
    {
        [SerializeField] private ProjectilePoolType _homingProjectilePoolType;
        [SerializeField] private int _maxProjectileCount = 5;
        [SerializeField] private float _fireDelay = 0.5f;
        [SerializeField] private Transform[] _firePoints;

        public override void OnStart()
        {
            base.OnStart();
            _giantGolemEye.LookAtPlayerDirection = true;
            EntityAnimator.OnAnimationTriggeredEvent += OnAnimationTriggeredHandle;
        }

        private void OnAnimationTriggeredHandle(EAnimationTriggerType triggertype, int count)
        {
            if (triggertype == EAnimationTriggerType.Trigger)
            {
                Boss.StartCoroutine(CoroutineFireProjectiles());
            }
        }

        private IEnumerator CoroutineFireProjectiles()
        {
            for (int i = 0; i < _maxProjectileCount; i++)
            {
                yield return new WaitForSeconds(_fireDelay);

                foreach (var firePoint in _firePoints)
                {
                    Vector2 direction = (Player.transform.position - firePoint.position).normalized;
                    HomingProjectile homingProjectile = PopCore.Pop(_homingProjectilePoolType, firePoint.position, Quaternion.identity) as HomingProjectile;
                    // TODO : 나중에 값 바꿔가며 수정
                    homingProjectile.Init(direction, 10, 10);
                }
            }

            yield return new WaitForSeconds(1);
            EndPattern();
        }


        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= OnAnimationTriggeredHandle;
            base.OnEnd();
        }
    }
}
