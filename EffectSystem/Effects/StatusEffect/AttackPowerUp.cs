using Hashira.Core.StatSystem;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class AttackPowerUp : Effect, ICoolTimeEffect
    {
        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }

        private StatModifier _attackSpeedUpModifier;
        private EModifyLayer _modifyLayer;
        private static int _ID;
        private int _id;

        public void Setup(StatModifier statModifier, float duration, EModifyLayer layer = EModifyLayer.Default)
        {
            _attackSpeedUpModifier = statModifier;
            _id = _ID++;
            Duration = duration;
            _modifyLayer = layer;
        }

        public override void Enable()
        {
            base.Enable();
            entityStat?.StatDictionary[StatName.AttackPower].AddModify($"{nameof(AttackSpeedUp)}Effect_{_id}", _attackSpeedUpModifier, _modifyLayer);
        }

        public override void Disable()
        {
            base.Disable();
            entityStat?.StatDictionary[StatName.AttackPower].RemoveModifyOverlap($"{nameof(AttackSpeedUp)}Effect_{_id}", _modifyLayer);
        }

        public void OnTimeOut()
        {

        }
    }
}
