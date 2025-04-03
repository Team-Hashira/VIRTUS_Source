using Crogen.CrogenPooling;
using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGoblinThrowPattern : GiantGoblinPattern
    {
        [SerializeField] private ProjectilePoolType _giantRockPoolType;
        [SerializeField] private int _rockStartLevel;
        [SerializeField] private Transform _rockFirePoint;
        private float _lookDirection;
        private int _rockCount;

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
            _rockCount = (int)Mathf.Pow(2, _rockStartLevel)-1;
        }

        private void HandleThrowRock(EAnimationTriggerType trigger, int count)
        {
            if (trigger != EAnimationTriggerType.Trigger) return;
            GiantBounceRock rock = GameObject.Pop(_giantRockPoolType, Transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360))) as GiantBounceRock;
            rock.Init(_rockStartLevel, Player.transform.position, HandleRocksDieEvent);
        }

        public override void OnEnd()
        {
            EntityAnimator.OnAnimationTriggeredEvent -= HandleThrowRock;
            base.OnEnd();
        }

        private void HandleRocksDieEvent(int level)
        {
            --_rockCount;
            if (_rockCount <= 0)
                EndPattern();
        }
    }
}
