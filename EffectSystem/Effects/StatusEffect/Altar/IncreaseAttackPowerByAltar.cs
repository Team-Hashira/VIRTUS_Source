using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseAttackPowerByAltar : AltarEffect
    {
        protected override EffectPoolType EffectVFX { get; } = EffectPoolType.IncreaseAttackPowerVFX;
        public override int MaxCount { get; set; } = 3;

        public override void Enable()
        {
            base.Enable();
            entityStat.StatDictionary["AttackPower"].AddModify("IncreaseAttackPowerByAltar", 10 ,EModifyMode.Add, EModifyLayer.Default);
        }

        public override void Disable()
        {
            entityStat.StatDictionary["AttackPower"].RemoveModifyOverlap("IncreaseAttackPowerByAltar", EModifyLayer.Default);
            base.Disable();
        }
    }
}