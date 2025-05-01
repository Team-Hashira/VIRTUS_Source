using Hashira.Core.StatSystem;
using System;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class AttackSpeedUp : Effect, ICoolTimeEffect
    {
        public float Duration { get; private set; }
        public float LifeTime { get; set; }
        public Action<Effect> OnTimeOutEvent { get; set; }

        private StatModifier _attackPowerUpModifier;
        private EModifyLayer _modifyLayer;
        private static int _ID;
        private int _id;

        public void Setup(StatModifier statModifier, float duration, EModifyLayer layer = EModifyLayer.Default)
        {
            _attackPowerUpModifier = statModifier;
            _id = _ID++;
            Duration = duration;
            _modifyLayer = layer;
        }

        public override void Enable()
        {
            base.Enable();
            entityStat.StatDictionary[StatName.AttackSpeed].AddModify($"{nameof(AttackPowerUp)}Effect_{_id}", _attackPowerUpModifier, _modifyLayer);
        }

        public override void Disable()
        {
            base.Disable();
            entityStat.StatDictionary[StatName.AttackSpeed].RemoveModifyOverlap($"{nameof(AttackPowerUp)}Effect_{_id}", _modifyLayer);
        }

        public void OnTimeOut()
        {
        }
    }
}
