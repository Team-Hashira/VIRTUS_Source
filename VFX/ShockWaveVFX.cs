using Crogen.CrogenPooling;
using Hashira.Entities;
using System;
using UnityEngine;

namespace Hashira
{
    public class ShockWaveVFX : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private LayerMask _whatIsHitable;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Vector2 _damageRange = new Vector2(1, 1);
        [SerializeField] private Vector2 _damageRangeOffset;
        
        public string OriginPoolType { get; set; }
        public new GameObject gameObject { get; set; }
        
        public void OnPop()
        {
        }

        public void OnPush()
        {
        }

        private void Update()
        {
            Vector3 moveDir = transform.rotation * Vector3.right;
            transform.position += moveDir * Time.deltaTime * _moveSpeed;
            DamageCast(moveDir);
        }

        private void DamageCast(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.BoxCast((Vector2)transform.position + _damageRangeOffset, _damageRange, 0, Vector2.zero, 0, _whatIsHitable);

            if (hit.transform == null) return;            
            
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(1, hit, transform, direction, EAttackType.Fixed, false);
                return;
            }
            
            this.Push();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)_damageRangeOffset, _damageRange);
        }
#endif
    }
}
