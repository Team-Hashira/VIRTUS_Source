using Hashira.Bosses.BillboardClasses;
using Hashira.Bosses.Patterns.GiantGolem;
using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemPattern : BossPattern
    {
        protected GiantGolemEye _giantGolemEye;
        protected Boss _handL;
        protected Boss _handR;
        protected Transform _smokeTransform;
        protected GiantGolemPlatformList _giantGolemPlatformList;
        protected Vector3 _originPosition;
        
        public override void Initialize(Boss boss)
        {
            base.Initialize(boss);
            _giantGolemEye = BillboardValue<GiantGolemEyeValue>("GiantGolemEye").Value;
            
            _handL = BillboardValue<BossValue>("GiantGolemHandL").Value;
            _handR = BillboardValue<BossValue>("GiantGolemHandR").Value;
            _smokeTransform = BillboardValue<TransformValue>("SmokeTransform").Value;
            _giantGolemPlatformList = BillboardValue<GiantGolemPlatformListValue>("GiantGolemPlatformList").Value;
            _originPosition = Transform.position;
        }
    }
}
