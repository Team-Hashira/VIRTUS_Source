using Hashira.Core.StatSystem;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class SpeedStatCard : CardEffect
    {
        private StatElement _speedStat;

        [SerializeField] private int[] _speedUpByStack = new int[] { 5, 8, 11, 15, 25 };

        public override void Enable()
        {
            base.Enable();
            _speedStat = player.GetEntityComponent<EntityStat>().StatDictionary[StatName.Speed];
            _speedStat.AddModify("SpeedStatEffect", _speedUpByStack[stack - 1], EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            base.Disable();
            _speedStat.RemoveModifyOverlap("SpeedStatEffect", EModifyLayer.Default);
        }

        public override void Update()
        {
        }
    }
}
