using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class MagicCardEffect : CardEffect
    {
        protected override int[] _NeedCostByStack { get; }

        protected float _delayMultiplier = 1f;

        protected virtual float DelayTime { get; }
        protected float _curTime = 0f;

        public override void Enable()
        {

        }

        public override void Disable()
        {

        }

        public override void Update()
        {
            _curTime += Time.deltaTime;
            if (DelayTime * _delayMultiplier < _curTime)
            {
                _curTime = 0;
                Use();
            }
        }

        public virtual void Use()
        {

        }

        public virtual void SetMultiplier(float multiplier)
        {
            _delayMultiplier = multiplier;
        }
        public virtual void DelayDown(float time, EModifyMode eModifyMode)
        {
            if (eModifyMode == EModifyMode.Percent)
            {
                _curTime += (DelayTime * _delayMultiplier - _curTime) * time / 100;
            }
            else
            {
                _curTime += time;
            }
        }
    }
}
