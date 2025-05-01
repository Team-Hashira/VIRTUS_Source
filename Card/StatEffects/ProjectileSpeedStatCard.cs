using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class ProjectileSpeedStatCard : CardEffect
    {
        [SerializeField] protected readonly float[] _projectileSpeedUpValue = { 30f, 50f, 70f };

        private StatElement _projectileSpeedStat;

        public override void Enable()
        {
            _projectileSpeedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.ProjectileSpeed];
            _projectileSpeedStat.AddModify("ProjectileSpeedStatEffect", _projectileSpeedUpValue[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _projectileSpeedStat?.RemoveModifyOverlap("ProjectileSpeedStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {

        }
    }
}
