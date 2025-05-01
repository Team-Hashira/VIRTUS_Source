using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Accessories.Effects
{
    [Serializable]
    public abstract class AccessoryActiveEffect : AccessoryEffect
    {
        public abstract void OnActivate();
    }
}
