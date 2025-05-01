using Hashira.Core.StatSystem;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class BlindlyCard : CardEffect
    {
        private StatDictionary _statDictionary;
        private StatElement _attackPowerStat;
        private StatElement _attackSpeedStat;

        public override void Enable()
        {
            _statDictionary = player.GetEntityComponent<EntityStat>().StatDictionary;
            _attackPowerStat = _statDictionary[StatName.AttackPower];
            _attackSpeedStat = _statDictionary[StatName.AttackSpeed];
            _attackPowerStat.AddModify("BlindlyCard", -80f, EModifyMode.Percent, EModifyLayer.Last);
            _attackSpeedStat.AddModify("BlindlyCard", 300f, EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _attackPowerStat.RemoveModifyOverlap("BlindlyCard", EModifyLayer.Last);
            _attackSpeedStat.RemoveModifyOverlap("BlindlyCard", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
