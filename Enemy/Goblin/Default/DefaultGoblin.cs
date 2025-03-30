using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using UnityEngine;

namespace Hashira.Enemies.Goblin.DefaultGoblin
{
    public class DefaultGoblin : Enemy
    {
        [field: SerializeField]
        public DamageCaster2D DamageCaster { get; private set; }

        protected override void Awake()
        {
            base.Awake();
        }
    }
}
