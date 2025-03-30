using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class ProjectileSpeedStatCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1 };
        protected override int[] _NeedCostByStack => _needCostByStack;
        protected readonly float[] _projectileSpeedUpValue = { 30f, 50f, 70f };

        private StatElement _projectileSpeedStat;

        public override void Enable()
        {
            _projectileSpeedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.ProjectileSpeed];
            _projectileSpeedStat.AddModify("ProjectileSpeedStatEffect", _projectileSpeedUpValue[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _projectileSpeedStat.RemoveModify("ProjectileSpeedStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {

        }
    }
}
