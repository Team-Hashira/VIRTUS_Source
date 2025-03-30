using Hashira.Entities;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public class ShieldHandler : DamageHandler
    {
        public int Shield { get; private set; } 

        public override EDamageHandlerStatus Calculate(int damage, EAttackType attackType, out int calculatedDamage)
        {
            calculatedDamage = damage - Shield;
            if (calculatedDamage < 0)
                calculatedDamage = 0;

            return EDamageHandlerStatus.Continue;
        }
    }
}
