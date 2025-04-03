using Crogen.CrogenPooling;
using DG.Tweening;
using Hashira.Combat;
using Hashira.Core;
using Hashira.Entities;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Hashira.Bosses.Patterns.GiantGolem
{
    public class GiantGolemObstacle : MonoBehaviour, IDamageable
    {
        [SerializeField] private AttackVisualizer _attackVisualizer;
        [SerializeField] private float _groundPositionY = -1f;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private EffectPoolType _destroyEffectPoolType;
        [FormerlySerializedAs("_gitCountText")] [FormerlySerializedAs("_HitCountText")] [SerializeField] private TextMeshPro _hitCountText;
        [SerializeField] private int _maxHitCount = 8;
        private Rigidbody2D _rigidbody;
        private int _currentHitCount = 0;
        public bool IsEvasion { get; set; }
        private readonly int _blinkShanderkHash = Shader.PropertyToID("_Blink");
        private List<Tween> _blinkTweenList = new List<Tween>();
        private bool _attackedGround = false;
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_attackedGround) return;
            GroundAttack();
        }

        private void LateUpdate()
        {
            _attackVisualizer.transform.rotation = Quaternion.identity;
            _attackVisualizer.transform.position = new Vector3(transform.position.x, _groundPositionY, transform.position.z);
            _hitCountText.transform.rotation = Quaternion.identity;
            _hitCountText.transform.position = transform.position;
        }

        private void GroundAttack()
        {
            _attackedGround = true;
            Vector2 playerPos = PlayerManager.Instance.Player.transform.position;

            _attackVisualizer.DamageCaster.CastDamage(
                AttackInfo.defaultOneDamage, 
                (playerPos - (Vector2)transform.position).normalized*8f, 
                false);

            CameraManager.Instance.ShakeCamera(10, 50, 0.5f);
            
            _attackVisualizer.SetDamageCastSignValue(0);
        }

        public void ApplyDamage(AttackInfo attackInfo, RaycastHit2D raycastHit, bool popUpText = true)
        {
            ++_currentHitCount;
            Blink(0.1f);
            if (_currentHitCount >= _maxHitCount)
            {
                gameObject.Pop(_destroyEffectPoolType, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
            _hitCountText.text = (_maxHitCount - _currentHitCount).ToString();
            _hitCountText.transform.DOShakePosition(0.1f, 1f, 40, fadeOut: false);
        }

        public void Throw(Vector2 targetPosition)
        {
            // 시작 위치와 목표 위치를 Vector2로 정의
            Vector2 startPosition = transform.position;

            // 원하는 비행시간 (예: 2초)
            float flightTime = 2f;

            // 중력의 영향을 계산하고, 초기 속도(또는 impulse)를 구함
            Vector2 force = (targetPosition - startPosition - 0.5f * Physics2D.gravity * _rigidbody.gravityScale * _rigidbody.mass * flightTime * flightTime) / flightTime;

            // Rigidbody2D에 impulse 방식으로 힘을 가함
            _rigidbody.AddForce(force, ForceMode2D.Impulse);
            
            _attackVisualizer.ResetDamageCastVisualSign();
            _attackVisualizer.SetDamageCastSignValue(1, 0.45f);
        }
        
        private void Blink(float duration, Ease ease = Ease.Linear)
        {
            foreach (Tween tween in _blinkTweenList)
            {
                tween.Kill();
            }
            _blinkTweenList.Clear();

            _renderer.material.SetFloat(_blinkShanderkHash, 1);
            _blinkTweenList.Add(_renderer.material.DOFloat(0, _blinkShanderkHash, duration).SetEase(ease));
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(-1000, _groundPositionY), new Vector3(1000, _groundPositionY));
        }
    }
}
