using Hashira.Combat;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public class LifeRingHandler : DamageHandler
    {
        public event Action OnHandlerCalledEvent;

        public override EDamageHandlerStatus Calculate(ref AttackInfo attackInfo)
        {
            int sub = _owner.GetEntityComponent<EntityHealth>().Health - attackInfo.damage;
            if (sub <= 0)
            {
                attackInfo.damage += sub - 1;
                OnHandlerCalledEvent?.Invoke();
                return EDamageHandlerStatus.Stop;
            }
            return EDamageHandlerStatus.Continue;
        }
    }
}
