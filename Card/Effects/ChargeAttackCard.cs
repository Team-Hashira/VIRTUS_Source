using Hashira.Core.StatSystem;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class ChargeAttackCard : CardEffect
    {
        [SerializeField] private float[] _maxChargeTimeByStack = { 2.5f, 2.3f, 2.1f };
        [SerializeField] private float[] _speedDownByStack = { 10f, 10f, 0f };

        private StatElement _speedStat;

        public override void Enable()
        {
            player.Attacker.SetChargeMode(_maxChargeTimeByStack[stack - 1]);
            player.Attacker.OnChargeEnableEvent += HandleChargeEnableEvent;

            _speedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.Speed];
        }

        private void HandleChargeEnableEvent(bool isCharging)
        {
            if (isCharging)
            {
                _speedStat.AddModify("ChargeAttackCard", -_speedDownByStack[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
            }
            else
            {
                _speedStat.RemoveModifyOverlap("ChargeAttackCard", EModifyLayer.Default);
            }
        }

        public override void Disable()
        {
            player.Attacker.OnChargeEnableEvent -= HandleChargeEnableEvent;
        }

        public override void Update()
        {

        }
    }
}
