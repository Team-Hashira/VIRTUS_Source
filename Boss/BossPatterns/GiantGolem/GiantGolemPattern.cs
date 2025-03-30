using Hashira.Bosses.BillboardClasses;

namespace Hashira.Bosses.Patterns
{
    public class GiantGolemPattern : BossPattern
    {
        protected GiantGolemHand _handL;
        protected GiantGolemHand _handR;
        
        protected LaserHandEffect _laserHandEffectL;
        protected LaserHandEffect _laserHandEffectR;
        
        protected GiantGolemEye _giantGolemEye;
        
        public override void Init(Boss boss)
        {
            base.Init(boss);
            _handL = Boss.BillboardValue<GiantGolemHandValue>("HandL").Value;
            _handR = Boss.BillboardValue<GiantGolemHandValue>("HandR").Value;
            _giantGolemEye = Boss.BillboardValue<GiantGolemEyeValue>("GiantGolemEye").Value;
        }

        public override void OnStart()
        {
            base.OnStart();
            _laserHandEffectL = _handL.GetHandEffect<LaserHandEffect>();
            _laserHandEffectR = _handR.GetHandEffect<LaserHandEffect>();
        }
    }
}
