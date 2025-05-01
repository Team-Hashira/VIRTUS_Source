using Crogen.CrogenPooling;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira
{
    public abstract class ProjectileBase : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        [SerializeField] protected ProjectileCollider2D _projectileCollider2D;

        protected virtual void FixedUpdate()
        {
            Move();
        }

        protected abstract void Move();

        public virtual void OnPop()
        {

        }

        public virtual void OnPush()
        {

        }
    }
}
