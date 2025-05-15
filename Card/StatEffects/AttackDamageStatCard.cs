using Hashira.Core.StatSystem;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class AttackDamageStatCard : CardEffect
    {
        [SerializeField] private readonly float[] _damageUpValue = { 20f, 35f, 50f, 100f };

        private StatElement _attackPowerStat;

        public override void Enable()
        {
            base.Enable();
            _attackPowerStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
            _attackPowerStat.AddModify("AttackPowerStatCard", _damageUpValue[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            base.Disable();
            _attackPowerStat.RemoveModifyOverlap("AttackPowerStatCard", EModifyLayer.Default);
        }
    }
}
