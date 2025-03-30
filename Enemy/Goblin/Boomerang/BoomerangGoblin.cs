using Crogen.CrogenPooling;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Enemies.Goblin.BoomerangGoblin
{
    public class BoomerangGoblin : Enemy
    {
        [field: SerializeField]
        public ProjectilePoolType Boomerang { get; private set; }
    }
}
