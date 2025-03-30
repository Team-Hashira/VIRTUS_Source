using Hashira.Combat;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Projectiles.Player
{
    public class PlayerBullet : Bullet
    {
        public override void Init(LayerMask whatIsTarget, Vector3 direction, float speed, int damage, Transform owner, bool isEventSender = true, float gravity = 0)
        {
            base.Init(whatIsTarget, direction, speed, damage, owner, isEventSender, gravity);
        }

        private void Update()
        {
            if (IsDead) return;

        }

        protected override void OnHited(HitInfo hitInfo)
        {
            base.OnHited(hitInfo);
        }

        public override void OnPop()
        {
            base.OnPop();
        }
    }
}
