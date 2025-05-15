using Hashira.Core;
using Hashira.Core.EventSystem;
using Hashira.EffectSystem.Effects;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira.Cards.Effects
{
    public class SharpProjectileCard : CardEffect
    {
        private float[] _percent = { 10, 15, 20 };

        public override void Enable()
        {
            base.Enable();
            GameEventChannel.AddListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }

        private void HandleProjectileHitEvent(ProjectileAfterHitEvent projectileHitEvent)
        {
            if (RandomUtility.RollChance(_percent[stack - 1]))
            {
                if (projectileHitEvent.hitInfo.entity != null && projectileHitEvent.hitInfo.entity.TryGetEntityComponent(out EntityEffector effecter))
                {
                    Bleeding bleeding = new Bleeding();
                    bleeding.Setup(5, 1f, nameof(SharpProjectileCard));
                    effecter.AddEffect(bleeding);
                }
            }
        }

        public override void Disable()
        {
            base.Disable();
            GameEventChannel.RemoveListener<ProjectileAfterHitEvent>(HandleProjectileHitEvent);
        }
    }
}
