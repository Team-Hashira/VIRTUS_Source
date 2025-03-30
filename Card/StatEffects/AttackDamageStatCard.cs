using Hashira.Core.StatSystem;
using Hashira.Entities;

namespace Hashira.Cards.Effects
{
    public class AttackDamageStatCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1, 1, 2 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private readonly float[] _damageUpValue = { 20f, 35f, 50f, 100f };

        private StatElement _attackPowerStat;

        public override void Enable()
        {
            _attackPowerStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackPower];
            _attackPowerStat.AddModify("AttackPowerStatCard", _damageUpValue[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _attackPowerStat.RemoveModify("AttackPowerStatCard", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
