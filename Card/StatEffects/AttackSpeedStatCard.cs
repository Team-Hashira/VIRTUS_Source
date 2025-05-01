using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class AttackSpeedStatCard : CardEffect
    {
        [SerializeField] protected readonly float[] _attackSpeedUpValue = { 20f, 30f, 50f };

        private StatElement _attackSpeedStat;

        public override void Enable()
        {
            _attackSpeedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.AttackSpeed];
            _attackSpeedStat.AddModify("AttackSpeedStatEffect", _attackSpeedUpValue[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _attackSpeedStat.RemoveModifyOverlap("AttackSpeedStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
