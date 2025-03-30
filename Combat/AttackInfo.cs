using Hashira.Entities;
using UnityEngine;

namespace Hashira.Combat
{
    public struct AttackInfo
    {
        public AttackInfo(
            int damage = 1, 
            RaycastHit2D raycastHit = default, 
            Transform attackerTrm = null, 
            Vector2 knockback = default, 
            Vector2 moveTo = default, 
            EAttackType attackType = EAttackType.Default)
        {
            this.damage = damage;
            this.raycastHit = raycastHit;
            this.attackerTrm = attackerTrm;
            this.knockback = knockback;
            this.moveTo = moveTo;
            this.attackType = attackType;
        }
        
        public int damage;
        public RaycastHit2D raycastHit;
        public Transform attackerTrm;
        public Vector2 knockback;
        public Vector2 moveTo;
        public EAttackType attackType;
    }
    
    public struct HitInfo
    {
        public Entity entity;
        public IDamageable damageable;
        public RaycastHit2D raycastHit;
    }
}
