using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Enemies;
using Hashira.Entities;
using Hashira.StageSystem;
using System;
using UnityEngine;

namespace Hashira
{
    public class Meteor : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private Vector2 _moveDir;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private LayerMask _whatIsTarget;
        [SerializeField] private LayerMask _whatIsObstacle;
        [SerializeField] private float _radius;
        [SerializeField] private CircleDamageCaster2D _circleDamageCaster2D;
        private Vector3 _targetPos;

        private bool _isPassedTarget;
        private float _startTime;
        private float _duration = 5f;
        private int _damage;

        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private void Update()
        {
            transform.position += (Vector3)_moveDir * Time.deltaTime;
            transform.rotation *= Quaternion.Euler(0, 0, _rotationSpeed * Time.deltaTime);

            if (_isPassedTarget == false && Vector3.Distance(_targetPos, transform.position) < 1f)
            {
                _isPassedTarget = true;
            }

            if (_isPassedTarget)
            {
                if (Physics2D.OverlapCircle(transform.position, _radius, _whatIsObstacle))
                {
                    Boom();
                }
            }
            else if (Physics2D.OverlapCircle(transform.position, _radius, _whatIsTarget))
            {
                Boom();
            }

            if (_startTime + _duration < Time.time)
            {
                this.Push();
            }
        }

        private void Boom()
        {
            AttackInfo attackInfo = new AttackInfo(_damage, Vector2.zero, EAttackType.Fire);
            _circleDamageCaster2D.CastDamage(attackInfo);
            PopCore.Pop(EffectPoolType.MeteorBoomEffect, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySFX("MeteorBoom", transform.position, 1f);
            CameraManager.Instance.ShakeCamera(10, 5, 0.4f);
            this.Push();
        }

        public void Init(Vector3 position, int damage)
        {
            _damage = damage;
            _targetPos = position;
            transform.position = _targetPos + new Vector3(-7, 7, 0);
            _isPassedTarget = false;
            _startTime = Time.time;
        }

        public void OnPop()
        {
            StageGenerator.Instance.OnNextStageEvent += HandleNextStage;
        }

        private void HandleNextStage()
        {
            this.Push();
        }

        public void OnPush()
        {
            StageGenerator.Instance.OnNextStageEvent += HandleNextStage;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
