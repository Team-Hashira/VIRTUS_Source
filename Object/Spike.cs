using Doryu.CustomAttributes;
using Hashira.Combat;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hashira.Object
{
    public class Spike : MonoBehaviour
    {
        [SerializeField] private float _attackDelay = 0.15f;
        private float _attackTimer;
        [SerializeField] private bool _onlyOnce = false;
        private bool _isWorked = false;
        [SerializeField] private bool _canDestroy = false;
        [SerializeField, ToggleField(nameof(_canDestroy))] private float _destroyDelay = 3f;

        private HashSet<IDamageable> _targets = new();

        private void OnTriggerEnter2D(Collider2D other)
        {
            var target = other.gameObject.GetComponent<IDamageable>();
            RaycastHit2D hit = new (){point = transform.position};
            target.ApplyDamage(AttackInfo.defaultOneDamage, hit, false);
            
            _targets.Add(target);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var target = other.gameObject.GetComponent<IDamageable>();
            if (_targets.Contains(target))
            {
                _targets.Remove(target);
            }
        }

        private void FixedUpdate()
        {
            if ((_onlyOnce && _isWorked) || (_canDestroy && _isWorked)) return;
            if (_attackTimer + _attackDelay < Time.time)
            {
                _attackTimer = Time.time;
                Attack();
            }
        }

        private void Attack()
        {
            foreach (var target in _targets)
            {
                RaycastHit2D hit = new (){point = transform.position};
                target.ApplyDamage(AttackInfo.defaultOneDamage, hit, false);
            }
            _isWorked = true;
            if (_canDestroy) Destroy(gameObject, _destroyDelay);
        }
    }
}
