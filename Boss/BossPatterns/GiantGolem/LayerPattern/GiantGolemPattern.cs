using Hashira.Bosses.BillboardClasses;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemPattern : BossPattern
    {
        protected GiantGolemEye _giantGolemEye;
        
        public override void Init(Boss boss)
        {
            base.Init(boss);
            _giantGolemEye = Boss.BillboardValue<GiantGolemEyeValue>("GiantGolemEye").Value;
        }
    }
}
