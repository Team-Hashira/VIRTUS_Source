using Crogen.CrogenPooling;

namespace Hashira.EffectSystem.Effects
{
    public class PaymentCost : Effect
    {
        private readonly EffectPoolType _PaymentCostVFXPoolType = EffectPoolType.HealVFX;

        private int _cost = 10;
        
        public void Setup(int cost) => _cost = cost;
        
        public override void Enable()
        {
            base.Enable();
            Cost.AddCost(_cost);
            PopCore.Pop(_PaymentCostVFXPoolType, entity.transform);
            entityEffector.RemoveEffect(this);
        }
    }
}