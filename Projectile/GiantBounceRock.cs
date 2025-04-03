using Crogen.CrogenPooling;
using Hashira.Combat;
using Hashira.Entities;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hashira.Bosses.Patterns
{
    public class GiantBounceRock : MonoBehaviour, IPoolingObject
    {
        [SerializeField] private LayerMask _whatIsPlayer;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private EffectPoolType _rockDestoryVFXPoolType;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _lowLevelStartAngle = 45f;
        public string OriginPoolType { get; set; }
        GameObject IPoolingObject.gameObject { get; set; }

        private Rigidbody2D _rigidbody;

        private int _level;
        private event Action<int> OnDieEvent;
        public float Mass => _rigidbody.mass;
        public float Gravity => _rigidbody.gravityScale;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Init(int level, Vector2 targetPos, Action<int> dieEvent)
        {
            this._level = level;

            // 시작 위치와 목표 위치를 Vector2로 정의
            Vector2 startPosition = transform.position;
            Vector2 targetPosition = targetPos;

            // 원하는 비행시간 (예: 2초)
            float flightTime = 1.8f;

            // 중력의 영향을 계산하고, 초기 속도(또는 impulse)를 구함
            Vector2 force = (targetPosition - startPosition - 0.5f * Physics2D.gravity * _rigidbody.gravityScale * flightTime * flightTime) / flightTime;

            // Rigidbody2D에 impulse 방식으로 힘을 가함
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
            
            transform.localScale = Vector3.one * _level;
            OnDieEvent = dieEvent;
        }
        public void Init(int level, Vector2 dir, float power, Action<int> dieEvent)
        {
            this._level = level;
            transform.localScale = Vector3.one * _level;
            _rigidbody.AddForce(dir.normalized * power, ForceMode2D.Impulse);
            OnDieEvent = dieEvent;
        }

        public void OnPop()
        {
            _rigidbody.linearVelocity = Vector2.zero;
        }

        public void OnPush()
        {
            gameObject.Pop(_rockDestoryVFXPoolType, transform.position, Quaternion.identity)
                .gameObject.transform.localScale = Vector3.one * _level;
        }

        private void FixedUpdate()
        {
            CheckGroundCollision();
            CheckDamageCast();
        }

        private void CheckDamageCast()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius * transform.localScale.z, Vector2.right, 0, _whatIsPlayer);

            if (hit.transform == null) return;

            if (hit.transform.TryGetComponent(out EntityHealth entityHealth))
            {
                entityHealth.ApplyDamage(AttackInfo.defaultOneDamage, hit, false);
            }
        }

        private void CheckGroundCollision()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, _radius * transform.localScale.z, Vector2.right, 0, _whatIsGround);
            if (hit.transform == null) return;

            // 벽에 맞았다면?
            Vector2 rDir = Quaternion.Euler(0, 0, -_lowLevelStartAngle) * hit.normal;
            Vector2 lDir = Quaternion.Euler(0, 0, _lowLevelStartAngle) * hit.normal;
            
            GenerateLowLevelRock(rDir, hit.normal);
            GenerateLowLevelRock(lDir, hit.normal);
            this.Push();
        }

        private void GenerateLowLevelRock(Vector2 dir, Vector2 normal)
        {
            OnDieEvent?.Invoke(_level);
            if (_level <= 1) return;
            var lowLevelRock = gameObject.Pop(OriginPoolType, transform.position, Quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 360))) as GiantBounceRock;

            float deceleration  = (-Vector2.Dot(_rigidbody.linearVelocity.normalized, normal) + 1) * 0.5f;
            lowLevelRock.Init(_level - 1, dir, _rigidbody.linearVelocity.magnitude * deceleration * 0.83f, OnDieEvent);
            
            
            // 충돌 노말 벡터랑 Vec.up Dot, 
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            float scale = Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius * scale);
            Gizmos.color = Color.white;
        }
#endif
    }
}
