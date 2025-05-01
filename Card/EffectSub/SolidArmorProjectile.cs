using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.Players;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira
{
    public class SolidArmorProjectile : MonoBehaviour, IPoolingObject, IEnumerable<int>
    {
        [SerializeField] private float _gravity;
        [SerializeField] private int _damage;
        [SerializeField] private int _speed;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private float _radius;

        private List<int> ahah;

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < ahah.Count; i++)
            {
                yield return ahah[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Vector3 _movement;
        private Player _player;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        public void Init(Player player, Vector3 dir)
        {
            _player = player;
            _movement = dir;
        }

        public void OnPop()
        {

        }

        public void OnPush()
        {

        }

        private void FixedUpdate()
        {
            Vector3 movement = _movement * (Time.fixedDeltaTime * _speed);

            RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, _radius, movement.normalized, movement.magnitude, _whatIsTarget);
            if (raycastHit)
            {
                if (raycastHit.transform.TryGetComponent(out Entity entity) &&
                entity.TryGetEntityComponent(out EntityHealth health))
                {
                    AttackInfo attackInfo = new AttackInfo(_damage);
                    health.ApplyDamage(attackInfo, raycastHit);
                }
                this.Push();
            }
            else
            {
                transform.position += _player.transform.rotation * movement;
                _movement.y -= _gravity * Time.fixedDeltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
