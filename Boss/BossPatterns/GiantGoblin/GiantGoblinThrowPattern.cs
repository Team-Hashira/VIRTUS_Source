using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using Hashira.Projectiles;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGoblinThrowPattern : BossPattern
    {
        [SerializeField] private Transform _firePoint;
        [SerializeField] private ProjectilePoolType _giantRockPoolType;
        [SerializeField] private int _rockStartLevel;
        [SerializeField] private Transform _rockFirePoint;
        private float _lookDirection;
        private int _rockCount;
        List<GiantBounceRock> _rockList;

        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _rockList = new List<GiantBounceRock>();
        }

        public override void OnStart()
        {
            base.OnStart();
            SetLookDirection();
            SetRockCount();

            EntityAnimator.OnAnimationTriggeredEvent += HandleThrowRock;
        }

        private void SetLookDirection()
        {
            _lookDirection = Mathf.Sign(Player.transform.position.x - Transform.position.x);
            EntityRenderer.LookTarget(_lookDirection);
        }

        private void SetRockCount()
        {
            _rockCount = 1;
            for (int i = 1; i < _rockStartLevel; i++)
                _rockCount += (int)Mathf.Pow(i, 2);
            Debug.Log(_rockCount);
        }

        private void HandleThrowRock(EAnimationTriggerType trigger, int count)
        {
            if (trigger != EAnimationTriggerType.Trigger) return;
            GiantBounceRock rock = PopCore.Pop(_giantRockPoolType, _firePoint.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GiantBounceRock;
            rock.Owner = Boss;
            rock.Init(_rockStartLevel, Player.transform.position, HandleRocksDieEvent);
        }

        public override void OnEnd()
        {
            _rockList?.Clear();
            EntityAnimator.OnAnimationTriggeredEvent -= HandleThrowRock;
            base.OnEnd();
        }

        private void HandleRocksDieEvent(IEnemyProjectile rock, int level)
        {
            _rockCount--;
            if (_rockCount <= 0)
            {
                EndPattern();
            }
        }
    }
}
