using Hashira.Entities;
using UnityEngine;

namespace Hashira.Core.DamageHandler
{
    public abstract class DamageHandler
    {
        public EDamageHandlerLayer SortingLayer { get; protected set; }
        public int OrderInLayer { get; protected set; }
        public abstract EDamageHandlerStatus Calculate(int damage, EAttackType attackType, out int calculatedDamage);
    }
}
