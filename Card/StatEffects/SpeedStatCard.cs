using Hashira.Core.StatSystem;
using Hashira.Entities;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class SpeedStatCard : CardEffect
    {
        private int[] _needCostByStack = new int[] { 1, 1, 1, 2 };
        protected override int[] _NeedCostByStack => _needCostByStack;

        private StatElement _speedStat;

        private int[] _speedUpByStack = new int[] { 5, 8, 11, 15, 25 };

        public override void Enable()
        {
            _speedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.Speed];
            _speedStat.AddModify("SpeedStatEffect", _speedUpByStack[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            _speedStat.RemoveModify("SpeedStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
