using Hashira.Combat;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public abstract class DamageHandler
    {
        protected Entity _owner;
        protected EntityHealth _ownerHealth;
        public EDamageHandlerLayer SortingLayer { get; protected set; }
        public int OrderInLayer { get; protected set; }

        public void SetOrderInLayer(int value)
        {
            OrderInLayer = value;
        }

        public virtual void Initialize(Entity owner)
        {
            _owner = owner;
            _ownerHealth = owner.GetEntityComponent<EntityHealth>();
        }
        public abstract EDamageHandlerStatus Calculate(ref AttackInfo attackInfo);
    }
}
