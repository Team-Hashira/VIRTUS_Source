using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.Players;
using Hashira.Projectiles;
using UnityEngine;

namespace Hashira.Enemies.Slime
{
    public class SlimeBall : MonoBehaviour, IPoolingObject
    {
        private Slime _slime;

        [SerializeField]
        private Rigidbody2D _rigidbody;
        [SerializeField]
        private ProjectileCollider2D _projectileCollider;
        [SerializeField]
        private float _flightTime;

        private RaycastHit2D[] _hits;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        public void Initialize(Slime slime, Vector3 targetPosition)
        {
            _slime = slime;

            Vector2 force = (targetPosition - transform.position - (Vector3)Physics2D.gravity * _rigidbody.gravityScale * 0.5f * _flightTime) / _flightTime;
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (_projectileCollider.CheckCollision(_slime.WhatIsPlayer, out _hits))
            {
                _hits[0].collider.GetComponent<Player>().GetEntityComponent<EntityHealth>().ApplyDamage(AttackInfo.defaultOneDamage);
                this.Push();
            }
        }

        public void OnPop()
        {
        }

        public void OnPush()
        {
        }
    }
}
