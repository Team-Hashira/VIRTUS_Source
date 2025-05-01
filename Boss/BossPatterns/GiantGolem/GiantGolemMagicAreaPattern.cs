using UnityEngine;

namespace Hashira.Bosses.Patterns
{
    [System.Serializable]
    public class GiantGolemMagicAreaPattern : GiantGolemPattern
    {       
        [SerializeField] private float _duration = 5f;
        private float _attackTime = 0;
        
        public override void OnStart()
        {
            base.OnStart();
            _attackTime = Time.time;

            float disL = _handL == null ? 1000 : Mathf.Abs(Player.transform.position.x - _handL.transform.position.x);
            float disR = _handR ==null ? 1000 : Mathf.Abs(Player.transform.position.x - _handR.transform.position.x);

            Boss magicAreaHand = null;
            Boss otherPatternHand = null;

            if (disL < disR)
            {
                magicAreaHand = _handL;
                otherPatternHand = _handR;
            }
            else
            {
                magicAreaHand = _handR;
                otherPatternHand = _handL;
            }

            magicAreaHand?.SetCurrentBossPattern<GiantGolemHandMagicAreaPattern>();
            otherPatternHand?.SetCurrentBossPattern<GiantGolemHandLaserPattern>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (_attackTime + _duration < Time.time)
            {
                Debug.Log($"_handL:{_handL!=null} _handR:{_handR!=null}");
                EndPattern();
            }
        }

        public override void OnEnd()
        {
            _handL?.CurrentBossPattern?.EndPattern();
            _handR?.CurrentBossPattern?.EndPattern();
            base.OnEnd();
        }
    }
}
