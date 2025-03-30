using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;

namespace Hashira.Cards.Effects
{
    public class HealthStatCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private StatElement _healthStat;

        public override void Enable()
        {
            _healthStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.Health];
            _healthStat.AddModify("HealthStatEffect", 1 + stack, EModifyMode.Add, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _healthStat.RemoveModify("HealthStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
