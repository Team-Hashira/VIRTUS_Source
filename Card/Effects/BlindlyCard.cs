using Hashira.Core.StatSystem;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class BlindlyCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

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
            _attackPowerStat.RemoveModify("BlindlyCard", EModifyLayer.Last);
            _attackSpeedStat.RemoveModify("BlindlyCard", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
