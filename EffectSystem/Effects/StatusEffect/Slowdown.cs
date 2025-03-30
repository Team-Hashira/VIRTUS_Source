using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class Slowdown : Effect, ICoolTimeEffect
    {
        public float Duration { get; private set; }
        public float LifeTime { get; set; }

        private StatModifier _slowStatModifier;
        private static int _ID;
        private int _id;

        public void Setup(StatModifier statModifier, float duration)
        {
            _slowStatModifier = statModifier;
            _id = _ID++;
            Duration = duration;
        }

        public override void Enable()
        {
            base.Enable();
            entityStat.StatDictionary[StatName.Speed].AddModify($"{name}_{_id}", _slowStatModifier, EModifyLayer.Default);
        }

        public override void Disable()
        {
            base.Disable();
            entityStat.StatDictionary[StatName.Speed].RemoveModify($"{name}_{_id}", EModifyLayer.Default);
        }

        public void OnTimeOut()
        {

        }
    }
}
