using Crogen.CrogenPooling;

namespace Hashira.EffectSystem.Effects
{
    public class PaymentCost : Effect
    {
        private int _cost = 10;
        
        public void Setup(int cost) => _cost = cost;
         
        public override void Enable()
        {
            base.Enable();
            Cost.AddCost(_cost);
            entityEffector.RemoveEffect(this);
        }
    }
}