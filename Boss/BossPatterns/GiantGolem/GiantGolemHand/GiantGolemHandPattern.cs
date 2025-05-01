using DG.Tweening;
using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns.GiantGolem;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemHandPattern : BossPattern
    {
        private Vector3 _originPosition;
        protected GiantGolemEye _giantGolemEye;
        protected GiantGolemPlatformList _giantGolemPlatformList;

        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _giantGolemEye = BillboardValue<GiantGolemEyeValue>("GiantGolemEye").Value;
            _giantGolemPlatformList = BillboardValue<GiantGolemPlatformListValue>("GiantGolemPlatformList").Value;

            _originPosition = Transform.position;
        }

        public Tween ReturnToOriginPosition(float duration = 0.5f)
        {
            return Transform.DOMove(_originPosition, duration);
        }

        public void MoveToTarget(Vector2 targetPosition, float duration = 0.85f)
        {
            Transform.DOMove(targetPosition, duration);            
        }
    }
}