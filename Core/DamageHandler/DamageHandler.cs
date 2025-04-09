using Hashira.Entities;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public abstract class DamageHandler
    {
        protected Entity _owner;
        public EDamageHandlerLayer SortingLayer { get; protected set; }
        public int OrderInLayer { get; protected set; }

        public virtual void Initialize(Entity owner)
            => _owner = owner;
        public abstract EDamageHandlerStatus Calculate(int damage, EAttackType attackType, out int calculatedDamage);
    }
}
