using UnityEngine;

namespace Hashira.Projectiles
{
    public abstract class ProjectileCollider2D : MonoBehaviour
    {
        public abstract bool CheckCollision(LayerMask whatIsTarget, out RaycastHit2D[] hits, Vector2 moveTo = default);
    }
}