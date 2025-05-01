using Crogen.CrogenPooling;
using Hashira.Entities;

namespace Hashira.EffectSystem.Effects
{
    public class Heal : Effect
    {
        private readonly EffectPoolType _healVFXPoolType = EffectPoolType.HealVFX;
        public int Amount { get; set; } = 2;
        
        public override void Enable()
        {
            base.Enable();
            entity.GetEntityComponent<EntityHealth>().ApplyRecovery(Amount);
            PopCore.Pop(_healVFXPoolType, entity.transform);
            entityEffector.RemoveEffect(this);
        }
    }
}