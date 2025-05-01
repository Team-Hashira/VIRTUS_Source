using Hashira.Entities;
using UnityEngine;

namespace Hashira.Combat
{
    public struct AttackInfo
    {
        public static AttackInfo defaultOneDamage =
            new AttackInfo(1, Vector2.zero, EAttackType.Default, null);

        public AttackInfo(
            int damage = 1,
            Vector2 knockback = default,
            EAttackType attackType = EAttackType.Default,
            IAttackable attacker = null
            )
        {
            this.damage = damage;
            this.knockback = knockback;
            this.attackType = attackType;
            this.attacker = attacker;
        }

        public int damage;
        public Vector2 knockback;
        public EAttackType attackType;
        public IAttackable attacker;
    }
    
    public struct HitInfo
    {
        public Entity entity;
        public IDamageable damageable;
        public RaycastHit2D raycastHit;
    }
}
