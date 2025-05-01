using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    [Serializable]
    public abstract class AccessoryEffect
    {
        protected Entity _owner;

        public virtual void Initialize(Entity owner)
            => _owner = owner;

        public virtual void OnUnequip() { }

        public virtual void Reset() { }
    }
}
