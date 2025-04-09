using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public class LifeRingHandler : DamageHandler
    {
        public event Action OnHandlerCalledEvent;

        public override EDamageHandlerStatus Calculate(int damage, EAttackType attackType, out int calculatedDamage)
        {
            calculatedDamage = damage;
            int sub = _owner.GetEntityComponent<EntityHealth>().Health - damage;
            if (sub <= 0)
            {
                calculatedDamage += sub - 1;
                OnHandlerCalledEvent?.Invoke();
                return EDamageHandlerStatus.Stop;
            }
            return EDamageHandlerStatus.Continue;
        }
    }
}
