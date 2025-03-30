using Hashira.Combat;
using Hashira.Entities;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Core.EventSystem
{
    public enum EWeaponType
    {
        Melee,
        Projectile
    }

    public static class InGameEvents
    {
        public static PlayerJumpEvent PlayerJumpEvent = new();
        public static PlayerHitEvent PlayerHitEvent = new();

        #region Projectile life cycle
        public static ProjectileShootEvent ProjectileShootEvent = new();
        public static ProjectileBeginHitEvent ProjectileBeginHitEvent = new();
        public static ProjectileAfterHitEvent ProjectileAfterHitEvent = new();
        #endregion

        public static KillEnemyEvent KillEnemyEvent = new();
    }

    public class PlayerJumpEvent : GameEvent 
    {
        public Vector3 jumpPoint;
    }
    public class ProjectileBeginHitEvent : GameEvent 
    {
        public HitInfo hitInfo;
        public Projectile projectile;
    }
    public class ProjectileAfterHitEvent : GameEvent
    {
        public HitInfo hitInfo;
        public Projectile projectile;
        public int appliedDamage;
    }
    public class ProjectileShootEvent : GameEvent 
    {
        public Projectile projectile;
        public bool isPlayerInput;
    }
    public class PlayerHitEvent : GameEvent
    {
        public EWeaponType weaponType;
    }

    public class KillEnemyEvent : GameEvent 
    {
        
    }
}
