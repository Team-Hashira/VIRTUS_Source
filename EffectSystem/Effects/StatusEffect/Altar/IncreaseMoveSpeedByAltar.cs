using Crogen.CrogenPooling;
using Hashira.Core.StatSystem;
using UnityEngine;

namespace Hashira.EffectSystem.Effects
{
    public class IncreaseMoveSpeedByAltar : AltarEffect
    {
        protected override EffectPoolType EffectVFX { get; } = EffectPoolType.IncreaseMoveSpeedVFX;
        public override int MaxCount { get; set; } = 3;

        public override void Enable()
        {
            base.Enable();
            entityStat.StatDictionary["Speed"].AddModify("IncreaseMoveSpeedByAltar", 10 ,EModifyMode.Percent, EModifyLayer.Default);
        }

        public override void Disable()
        {
            entityStat.StatDictionary["Speed"].RemoveModifyOverlap("IncreaseMoveSpeedByAltar", EModifyLayer.Default);
            base.Disable();
        }
    }
}