using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using Hashira.Projectiles;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Hashira.Enemies.Goblin.BoomerangGoblin
{
    public class GoblinBoomerang : MonoBehaviour, IPoolingObject
    {
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private BoomerangGoblin _boomerangGoblin;
        private Transform _goblinTrm;
        private Vector2 _lastGoblinPosition;
        private Vector2 _destination;

        public event Action OnReturnEvent;

        [SerializeField] 
        private ProjectileCollider2D _projectileCollider;
        [SerializeField] 
        private float _throwSpeed = 1.5f;
        [SerializeField] 
        private float _returnSpeed = 1.5f;
        [SerializeField] 
        private int _damage = 1;

        private enum EBoomerangState
        {
            Throwing,
            Returning,
            Inactive
        }

        private RaycastHit2D[] _hits;

        private EBoomerangState _currentState = EBoomerangState.Inactive;
        private bool _hasDealtDamage = false;

        [SerializeField]
        private float _rotatePerSecond = 180f;

        public void Initialize(BoomerangGoblin boomerangGoblin, Vector2 destination)
        {
            _boomerangGoblin = boomerangGoblin;
            _goblinTrm = boomerangGoblin.transform;
            _destination = destination;
            _lastGoblinPosition = _goblinTrm.position;
            _currentState = EBoomerangState.Throwing;
            _hasDealtDamage = false;

            StartCoroutine(MovementCoroutine());
        }

        private void HandleCollision(RaycastHit2D hit)
        {
            if (_hasDealtDamage || _currentState == EBoomerangState.Inactive) return;

            var entity = hit.collider.GetComponent<Entity>();
            if (entity != null)
            {
                var health = entity.GetEntityComponent<EntityHealth>();
                if (health != null)
                {
                    AttackInfo attackInfo = new AttackInfo(_damage);
                    health.ApplyDamage(attackInfo, popUpText: false);
                    _hasDealtDamage = true;
                }
            }
        }

        private void Update()
        {
            if (_goblinTrm != null)
                _lastGoblinPosition = _goblinTrm.position;
            if (_projectileCollider.CheckCollision(_boomerangGoblin.WhatIsPlayer, out _hits))
                HandleCollision(_hits[0]);
            transform.Rotate(Vector3.forward, _rotatePerSecond * Time.deltaTime);
        }

        private IEnumerator MovementCoroutine()
        {
            _currentState = EBoomerangState.Throwing;
            float percent = 0f;
            Vector2 startPos = transform.position;

            while (percent < 1f)
            {
                percent += Time.deltaTime * _throwSpeed;
                float t = MathEx.OutCubic(percent);
                transform.position = Vector2.Lerp(startPos, _destination, t);
                yield return null;
            }

            _currentState = EBoomerangState.Returning;
            percent = 0f;
            _hasDealtDamage = false;
            Vector2 returnDestination = _goblinTrm != null ? (Vector2)_goblinTrm.position : _lastGoblinPosition;

            while (percent < 1f)
            {
                percent += Time.deltaTime * _returnSpeed;
                float t = MathEx.InCubic(percent);

                if (_goblinTrm != null)
                    returnDestination = _goblinTrm.position;

                transform.position = Vector2.Lerp(_destination, returnDestination, t);
                yield return null;
            }

            _currentState = EBoomerangState.Inactive;
            if (_boomerangGoblin != null)
                OnReturnEvent?.Invoke();

            this.Push();
        }

        public void OnPop()
        {
            OnReturnEvent = null;
            _goblinTrm = null;
            _boomerangGoblin = null;
            _hasDealtDamage = false;
            _currentState = EBoomerangState.Inactive;
        }

        public void OnPush()
        {
        }
    }
}
