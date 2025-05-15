using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;

namespace Hashira.Cards.Effects
{
    public class HealthStatCard : CardEffect
    {
        private StatElement _healthStat;

        public override void Enable()
        {
            base.Enable();
            _healthStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.Health];
            _healthStat.AddModify("HealthStatEffect", 1 + stack, EModifyMode.Add, EModifyLayer.Default);
        }

        public override void Disable()
        {
            base.Disable();
            _healthStat.RemoveModifyOverlap("HealthStatEffect", EModifyLayer.Default);
        }
    }
}
