using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public abstract class MagicCardEffect : CardEffect
    {
        protected float _delayMultiplier = 1f;

        protected abstract float DelayTime { get; }
        protected float _curTime = 0f;

        public override void Enable()
        {
            base.Enable();
            _curTime = 0f;
        }

        public override void Update()
        {
            _curTime += Time.deltaTime;
            if (DelayTime * _delayMultiplier < _curTime)
            {
                _curTime = 0;
                OnUse();
            }
        }

        public abstract void OnUse();

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
