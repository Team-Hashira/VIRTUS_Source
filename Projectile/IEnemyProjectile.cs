using Hashira.Enemies;
using System;
using UnityEngine;

namespace Hashira.Projectiles
{
    public interface IEnemyProjectile
    {
        public Enemy Owner { get; }
        
        public bool CanAttack { get; }
    }
}